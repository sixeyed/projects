using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Heartbeat.Extensions;
using Sixeyed.Heartbeat.TestFramework;
using hb = Sixeyed.Heartbeat;

namespace Sixeyed.Heartbeat.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class HeartbeatTest
    {
        [TestMethod]
        public void Start_NoIntervals()
        {
            var heartbeat = new hb.Heartbeat(this);
            heartbeat.Start("Start_NoIntervals started");
            heartbeat.SetComplete("Start_NoIntervals completed");
        }

        [TestMethod]
        public void Start_NoIntervals_NoText()
        {
            var heartbeat = new hb.Heartbeat(this);
            heartbeat.Start();
            heartbeat.SetComplete();
        }

        [TestMethod]
        public void Start_NoIntervals_NoText_Failed()
        {
            var heartbeat = new hb.Heartbeat(this);
            heartbeat.Start();
            heartbeat.SetFailed();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "InitiatingObject must be set\r\nParameter name: InitiatingObject")]
        public void Start_InvalidState_NoInitiatingObject()
        {
            var heartbeat = new hb.Heartbeat();
            heartbeat.PulseCountInterval = 10;
            heartbeat.Start();
        }

        [TestMethod]
        public void RunTimer_NoHandler()
        {
            double timerInterval = 3 * 1000; //3 seconds
            var runTime = RandomValueGenerator.GetRandomInt(3 * 1000, 10 * 1000);
            var heartbeat = new hb.Heartbeat(this, 0, timerInterval);
            heartbeat.Start("RunTimer_NoHandler started, timerInterval: {0}, runTime: {1}".FormatWith(timerInterval, runTime));
            Thread.Sleep(runTime);
            heartbeat.SetComplete("RunTimer_NoHandler finished");
        }

        [TestMethod]
        public void RunTimer()
        {
            double timerInterval = 3 * 1000; //3 seconds
            var runTime = RandomValueGenerator.GetRandomInt(3 * 1000, 10 * 1000);
            var heartbeat = new hb.Heartbeat(this, 0, timerInterval);
    heartbeat.OnPulse += new Heartbeat.OnPulseEventHanlder(RunTimer_OnPulse);
    heartbeat.Start("RunTimer started, timerInterval: {0}, runTime: {1}".FormatWith(timerInterval, runTime));
            Thread.Sleep(runTime);
            heartbeat.SetComplete("RunTimer finished");
        }

    void RunTimer_OnPulse(PulseEventSource source, ref bool writeLog, ref string logText)
    {
        writeLog = true;
        logText = "RunTimer_OnPulse, source: {0}, text: {1}"
                    .FormatWith(source, RandomValueGenerator.GetRandomString());
    }

        [TestMethod]
        public void RunCount_NoHandler()
        {
            long countInterval = 5000;
            long countTo = RandomValueGenerator.GetRandomInt(1000, 50000);
            var heartbeat = new hb.Heartbeat(this, countInterval, 0);
    heartbeat.Start("RunCount_NoHandler, countInterval: {0}, countTo: {1}".FormatWith(countInterval, countTo));
            for (int i = 0; i < countTo; i++)
            {
                heartbeat.IncrementCount();
            }
            heartbeat.SetComplete("RunCount_NoHandler finished");
        }

        [TestMethod]
        public void RunCount_Failure()
        {
            long countInterval = 5000;
            long countTo = RandomValueGenerator.GetRandomInt(1000, 50000);
    var heartbeat = new hb.Heartbeat(this, countInterval, 0);
    heartbeat.Start("RunCount_NoHandler, countInterval: {0}, countTo: {1}".FormatWith(countInterval, countTo));
    try
    {
        for (int i = 0; i < countTo; i++)
        {
            heartbeat.IncrementCount();
        }
        var zero = 0;
        var dbz = 1 / zero;
        heartbeat.SetComplete("RunCount_NoHandler finished");
    }
    catch (Exception ex)
    {
        heartbeat.SetFailed("RunCount_NoHandler failed, message: {0}".FormatWith(ex.FullMessage()));
    }
        }

        [TestMethod]
        public void RunCount()
        {
            long countInterval = 5000;
            long countTo = RandomValueGenerator.GetRandomInt(1000, 50000);
            using (var heartbeat = new hb.Heartbeat(this, countInterval, 0))
            {
                heartbeat.OnPulse += new Sixeyed.Heartbeat.Heartbeat.OnPulseEventHanlder(RunCount_OnPulse);
                heartbeat.Start("RunCount, countInterval: {0}, countTo: {1}".FormatWith(countInterval, countTo));
                for (int i = 0; i < countTo; i++)
                {
                    heartbeat.IncrementCount();
                }
                heartbeat.SetComplete("RunCount finished");
            }
        }

        [TestMethod]
        public void RunCount_NoCompletion()
        {
            long countInterval = 5000;
            long countTo = RandomValueGenerator.GetRandomInt(1000, 50000);
            using (var heartbeat = new hb.Heartbeat(this, countInterval, 0))
            {
                heartbeat.Start("RunCount_NoCompletion, countInterval: {0}, countTo: {1}".FormatWith(countInterval, countTo));
                for (int i = 0; i < countTo; i++)
                {
                    heartbeat.IncrementCount();
                }
            }
        }

        void RunCount_OnPulse(PulseEventSource source, ref bool writeLog, ref string logText)
        {
            writeLog = true;
            logText = "RunCount_OnPulse, source: {0}, text: {1}"
                        .FormatWith(source, RandomValueGenerator.GetRandomString());
        }

        [TestMethod]
        public void RunCountAndTimer_NoHandler()
        {
            long countInterval = 1000;
            long countTo = RandomValueGenerator.GetRandomInt(1000, 10000);
            double timerInterval = 800; //0.8 seconds
            var runTime = RandomValueGenerator.GetRandomInt(3 * 1000, 10 * 1000);
            var heartbeat = new hb.Heartbeat(this, countInterval, timerInterval);
            heartbeat.Start("RunCountAndTimer_NoHandler, countInterval: {0}, countTo: {1}, timerInterval: {2}, runTime: {3}"
                                .FormatWith(countInterval, countTo, timerInterval, runTime));
            for (int i = 0; i < countTo; i++)
            {
                heartbeat.IncrementCount();
                if (RandomValueGenerator.GetRandomBool())
                {
                    Thread.Sleep(RandomValueGenerator.GetRandomInt(2));
                }
            }
            heartbeat.SetComplete("RunCountAndTimer_NoHandler finished");
        }

        private hb.Heartbeat _heartbeat;

        [TestMethod]
        public void RunCountAndTimer_NoHandler_ThreadPool()
        {
            long countInterval = 7000;
            long countTo = RandomValueGenerator.GetRandomInt(10000, 30000);
            double timerInterval = 300; //0.3 seconds
            var runTime = RandomValueGenerator.GetRandomInt(3 * 1000, 10 * 1000);
            _heartbeat = new hb.Heartbeat(this, countInterval, timerInterval);
            _heartbeat.Start("RunCountAndTimer_NoHandler_ThreadPool, countInterval: {0}, countTo: {1}, timerInterval: {2}, runTime: {3}"
                                .FormatWith(countInterval, countTo, timerInterval, runTime));
            for (int i = 0; i < countTo; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(Increment), null);
            }
            _heartbeat.SetComplete("RunCountAndTimer_NoHandler_ThreadPool finished");
        }

        [TestMethod]
        public void RunCountAndTimer_NoHandler_MultiThreaded()
        {
            long countInterval = 7000;
            long countTo = RandomValueGenerator.GetRandomInt(10000, 30000);
            double timerInterval = 300; //0.3 seconds
            var runTime = RandomValueGenerator.GetRandomInt(3 * 1000, 10 * 1000);
            _heartbeat = new hb.Heartbeat(this, countInterval, timerInterval);
            _heartbeat.Start("RunCountAndTimer_NoHandler_MultiThreaded, countInterval: {0}, countTo: {1}, timerInterval: {2}, runTime: {3}"
                                .FormatWith(countInterval, countTo, timerInterval, runTime));
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < countTo; i++)
            {
                Thread thread = new Thread(Increment);
                thread.Start();
            }
            // Wait for the threads.
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            threads.Clear();
            _heartbeat.SetComplete("RunCountAndTimer_NoHandler_MultiThreaded finished");
        }

        private void Increment(object state)
        {
            _heartbeat.IncrementCount();
            if (RandomValueGenerator.GetRandomBool())
            {
                Thread.Sleep(RandomValueGenerator.GetRandomInt(2));
            }
        }

        [TestMethod]
        public void RunCountAndTimer_NoHandler_WithTasks()
        {

            long countTo = RandomValueGenerator.GetRandomInt(20000, 50000);
            long countInterval = 7000;
            double timerInterval = 300; //0.3 seconds
            var heartbeat = new Heartbeat(this, countInterval, timerInterval);
            heartbeat.Start("RunCountAndTimer_NoHandler_WithTasks, countInterval: {0}, countTo: {1}, timerInterval: {2}"
                                .FormatWith(countInterval, countTo, timerInterval));
            var tasks = new Task[countTo];
            long finalTaskIndex = countTo - 1;
            for (long i = 0; i < countTo; i++)
            {
                var taskIndex = i;
                tasks[i] = Task.Factory.StartNew(() => DoWork(heartbeat, taskIndex, finalTaskIndex));
            }
            //for long-running async calls, this wouldn't be used, but
            //the unit test will not allow all threads to complete otherwise:
            Task.WaitAll(tasks);
        }

        private void DoWork(Heartbeat heartbeat, long taskIndex, long finalTaskIndex)
        {
            heartbeat.IncrementCount();
            //fake work:
            if (RandomValueGenerator.GetRandomBool())
            {
                Thread.Sleep(RandomValueGenerator.GetRandomInt(2));
            }            
            if (taskIndex == finalTaskIndex)
            {
                heartbeat.SetComplete("RunCountAndTimer_NoHandler_WithTasks finished");
            }
        }
    }
}
