using System;
using System.Timers;
using Sixeyed.Heartbeat.Repositories;
using Sixeyed.Heartbeat.Entities.Enums;
using Sixeyed.Heartbeat.Entities;
using Sixeyed.Heartbeat.Extensions;
using Sixeyed.Heartbeat.Configuration;
using Sixeyed.Heartbeat.Logging;

namespace Sixeyed.Heartbeat
{
    /// <summary>
    /// Heartbeat object which monitors for events and logs at set intervals
    /// </summary>
    /// <remarks>
    /// Intervals can be set as an elapsed time period, a count of work items, or both
    /// </remarks>
    public class Heartbeat : IDisposable
    {
        #region Private instance fields

        private bool _loggedEnd;
        private Timer _timer;
        private object _initiatingObject;
        private string _intiatingTypeName;
        private DateTime _startTime;

        private double _timerMilliseconds;
        private long _count;
        private object _countSyncLock = new object();

        private int _timerPulseNumber = 0;
        private object _timerPulseSyncLock = new object();

        private int _countPulseNumber = 0;
        private object _countPulseSyncLock = new object();

        #endregion

        #region Delegates & events

        /// <summary>
        /// Event handler delegate invoked on a pulse
        /// </summary>
        /// <param name="source">Source of the pulse</param>
        /// <param name="writeLog">Whether to write the pulse to the log database</param>
        /// <param name="logText">Optional text to perist to the log database</param>
        public delegate void OnPulseEventHanlder(PulseEventSource source, ref bool writeLog, ref string logText);

        /// <summary>
        /// Event fired when the heartbeat reaches the set count or timer increment
        /// </summary>
        public event OnPulseEventHanlder OnPulse;

        #endregion

        /// <summary>
        /// Static constructor - initialises data context
        /// </summary>
        static Heartbeat()
        {
			if (HeartbeatConfiguration.Current.Enabled)
            {
            	//run a dummy query to warm up EF:
            	HeartbeatRepository.GetHeartbeatLogs(new Guid());
			}
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Heartbeat()
        {
            HeartbeatInstanceId = Guid.NewGuid();
			PulseCountInterval = -1;
            PulseTimerInterval = -1;
        }

        /// <summary>
        /// Constructor with known state
        /// </summary>
        /// <param name="initiatingObject">Object using the heartbeat</param>
        public Heartbeat(object initiatingObject)
            : this()
        {
            InitiatingObject = initiatingObject;
        }

        /// <summary>
        /// Constructor with known state
        /// </summary>
        /// <param name="initiatingObject">Object using the heartbeat</param>
        /// <param name="pulseCountInterval">Interval at which the incremented count causes a pulse</param>
        /// <param name="pulseTimerInterval">Interval at which the timer fires to causes a pulse</</param>
        public Heartbeat(object initiatingObject, long pulseCountInterval, double pulseTimerInterval)
            : this(initiatingObject)
        {
            PulseCountInterval = pulseCountInterval;
            PulseTimerInterval = pulseTimerInterval;
        }
     
        /// <summary>
        /// Gets/sets the interval at which the timer fires to causes a pulse
        /// </summary>
        public double PulseTimerInterval { get; set; }

        /// <summary>
        /// Gets/sets the interval at which the incremented count causes a pulse
        /// </summary>
        public long PulseCountInterval { get; set; }

        /// <summary>
        /// Gets/sets the object using the heartbeat
        /// </summary>
        public object InitiatingObject
        {
            get { return _initiatingObject; }
            set
            {
                _initiatingObject = value;
                _intiatingTypeName = value.GetType().AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Returns the unique ID for this heartbeat
        /// </summary>
        public Guid HeartbeatInstanceId { get; private set; }

        /// <summary>
        /// Starts monitoring
        /// </summary>
        public void Start()
        {
            Start(string.Empty);
        }

        /// <summary>
        /// Starts the heartbeat with text to log
        /// </summary>
        /// <param name="logText">Text to log for the start event</param>
        public void Start(string logText)
        {
            if (!HeartbeatConfiguration.Current.Enabled)
            {
                Log.Warn(() => "Heartbeat instance id: {0}, not enabled in configuration. Not started"
                                    .FormatWith(HeartbeatInstanceId));
                return;
            }
            
            if (InitiatingObject == null)
            {
                throw new ArgumentException("InitiatingObject must be set", "InitiatingObject");
            }            

            if (PulseTimerInterval < 0)
            {
                PulseTimerInterval = HeartbeatConfiguration.Current.DefaultPulseTimerInterval;
                Log.Info(() => "Heartbeat instance id: {0}, using DefaultPulseTimerInterval: {1}"
                                .FormatWith(HeartbeatInstanceId, PulseTimerInterval));
            }
            if (PulseCountInterval < 0)
            {
                PulseCountInterval = HeartbeatConfiguration.Current.DefaultPulseCountInterval;
                Log.Info(() => "Heartbeat instance id: {0}, using DefaultPulseCountInterval: {1}"
                                .FormatWith(HeartbeatInstanceId, PulseCountInterval));
            }

            _startTime = DateTime.Now;

            if (PulseCountInterval > 0)
            {
                lock (_countSyncLock)
                {
                    _count = 0;
                }
            }

            if (PulseTimerInterval > 0)
            {
                _timer = new Timer(PulseTimerInterval);
                _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
                _timer.Start();
            }

            WriteLog(PulseEventSource.Start, HeartbeatStatus.Started, logText);
        }

        /// <summary>
        /// Increments the recorded count, triggering a pulse if the count meets the set interval
        /// </summary>
        public void IncrementCount()
        {
            lock (_countSyncLock)
            {
                _count++;
            }
            if (PulseCountInterval > 0 && (_count % PulseCountInterval == 0))
            {
                Pulse(PulseEventSource.Count);
            }
        }

        /// <summary>
        /// Stops monitoring, flagging the work as complete
        /// </summary>
        public void SetComplete()
        {
            SetComplete(string.Empty);
        }

        /// <summary>
        /// Stops monitoring, flagging the work as complete
        /// </summary>
        /// <param name="logText">Text to log for the completion event</param>
        public void SetComplete(string logText)
        {
            End(HeartbeatStatus.CompletedSucessfully, logText);
        }

        /// <summary>
        /// Stops monitoring, flagging the work as failed
        /// </summary>
        public void SetFailed()
        {
            SetFailed(string.Empty);
        }

        /// <summary>
        /// Stops monitoring, flagging the work as failed
        /// </summary>
        /// <param name="logText">Text to log for the failure event</param>
        public void SetFailed(string logText)
        {
            End(HeartbeatStatus.CompletedButFailed, logText);
        }

        #region Private instance methods

        private void End(HeartbeatStatus status, string logText)
        {
            StopTimings();
            WriteLog(PulseEventSource.End, status, logText);
            _loggedEnd = true;
        }

        private void StopTimings()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
            _timerMilliseconds = GetElapsedMilliseconds(DateTime.Now);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timerMilliseconds = GetElapsedMilliseconds(e.SignalTime);
            Pulse(PulseEventSource.Timer);
        }

        private double GetElapsedMilliseconds(DateTime instant)
        {
            var elapsedSpan = new TimeSpan((instant.Subtract(_startTime)).Ticks);
            return elapsedSpan.TotalMilliseconds;
        }

        private void Pulse(PulseEventSource pulseEventSource)
        {
            if (HeartbeatConfiguration.Current.Enabled)
            {
                switch (pulseEventSource)
                {
                    case PulseEventSource.Count:
                        lock (_countPulseSyncLock)
                        {
                            _countPulseNumber++;
                        }
                        break;
                    case PulseEventSource.Timer:
                        lock (_timerPulseSyncLock)
                        {
                            _timerPulseNumber++;
                        }
                        break;
                }
                var writeLog = true;
                var logText = string.Empty;
                if (OnPulse != null)
                {
                    OnPulse(pulseEventSource, ref writeLog, ref logText);
                }
                if (writeLog)
                {
                    WriteLog(pulseEventSource, HeartbeatStatus.InProgress, logText);
                }
            }
        }

        private void WriteLog(PulseEventSource pulseEventSource, HeartbeatStatus status, string logText)
        {
            if (HeartbeatConfiguration.Current.Enabled)
            {
                var log = new HeartbeatLog();
                log.LogDate = DateTime.Now;
                log.Status = status;
                log.HeartbeatInstanceId = HeartbeatInstanceId;
                log.ComponentTypeName = _intiatingTypeName;
                log.LogText = logText;
                if (pulseEventSource != PulseEventSource.Count)
                {
                    log.PulseTimerInterval = PulseTimerInterval;
                    log.TimerPulseNumber = _timerPulseNumber;
                    log.TimerMilliseconds = _timerMilliseconds;
                }
                if (pulseEventSource != PulseEventSource.Timer)
                {
                    log.PulseCountInterval = PulseCountInterval;
                    log.CountPulseNumber = _countPulseNumber;
                    log.CountNumber = _count;
                }
                HeartbeatRepository.WriteHeartbeatLog(log);
            }
        }

        #endregion

        #region IDisposable

        private bool _disposed;

        /// <summary>
        /// Cleans up resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up resources
        /// </summary>
        /// <param name="disposing">Flag if being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    StopTimings();
                    if (_timer != null)
                    {
                        _timer.Dispose();
                        _timer = null;
                    }
                    if (!_loggedEnd)
                    {
                        WriteLog(PulseEventSource.End, HeartbeatStatus.Unknown, "Heartbeat disposed without SetComplete() or SetFailed() called");
                    }
                    _disposed = true;
                }
            }
        }

        #endregion 
    }
}
