using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Sixeyed.Heartbeat.Extensions;
using Sixeyed.Heartbeat.TestFramework;

namespace Sixeyed.Heartbeat.Tests.Scenarios.Stubs
{
    public class StubComponent : IDisposable
    {
        private Heartbeat _heartbeat;

        public void Process()
        {
            long countInterval = 7000;
            long countTo = RandomValueGenerator.GetRandomInt(20000, 50000);
            double timerInterval = 300; //0.3 seconds
            _heartbeat = new Heartbeat(this, countInterval, timerInterval);
            _heartbeat.Start("StubComponent.Process started, countInterval: {0}, countTo: {1}, timerInterval: {2}"
                                .FormatWith(countInterval, countTo, timerInterval));
            var tasks = new Task[countTo];
            long finalTaskIndex = countTo - 1;
            for (long i = 0; i < countTo; i++)
            {
                var heartbeat = _heartbeat; //don't pass the instance directly
                var taskIndex = i;
                tasks[i] = Task.Factory.StartNew(() => DoWork(heartbeat, taskIndex, finalTaskIndex));
            }
        }

        private void DoWork(Heartbeat heartbeat, long taskIndex, long finalTaskIndex)
        {
            if (heartbeat == null)
                Trace.WriteLine("Heartbeat is null");

            heartbeat.IncrementCount();
            //fake work:
            if (RandomValueGenerator.GetRandomBool())
            {
                Thread.Sleep(RandomValueGenerator.GetRandomInt(100));
            }
            if (taskIndex == finalTaskIndex)
            {
                heartbeat.SetComplete("StubComponent.Process finished");
            }
        }

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
                    if (_heartbeat != null)
                    {
                        _heartbeat.Dispose();
                        _heartbeat = null;
                    }
                    _disposed = true;
                }
            }
        }

        #endregion
    }
}
