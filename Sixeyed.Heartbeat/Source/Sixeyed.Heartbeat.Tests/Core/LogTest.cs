using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Heartbeat.Extensions;
using Sixeyed.Heartbeat.Logging;

namespace Sixeyed.Heartbeat.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void Debug()
        {
            Log.Debug(() => "logging DEBUG with lambda");
            Log.Debug(() => "logging DEBUG with lambda: {0}".FormatWith(Guid.NewGuid()));
            Log.Debug("logging DEBUG without lambda");
            Log.Debug("logging DEBUG without lambda: {0}", Guid.NewGuid());
        }

        [TestMethod]
        public void Info()
        {
            Log.Info(() => "logging INFO with lambda");
            Log.Info(() => "logging INFO with lambda: {0}".FormatWith(Guid.NewGuid()));
            Log.Info("logging INFO without lambda");
            Log.Info("logging INFO without lambda: {0}", Guid.NewGuid());
        }

        [TestMethod]
        public void Warn()
        {
            Log.Warn(() => "logging WARN with lambda");
            Log.Warn(() => "logging WARN with lambda: {0}".FormatWith(Guid.NewGuid()));
            Log.Warn("logging WARN without lambda");
            Log.Warn("logging WARN without lambda: {0}", Guid.NewGuid());
        }

        [TestMethod]
        public void Error()
        {
            Log.Error(() => "logging ERROR with lambda");
            Log.Error(() => "logging ERROR with lambda: {0}".FormatWith(Guid.NewGuid()));
            Log.Error("logging ERROR without lambda");
            Log.Error("logging ERROR without lambda: {0}", Guid.NewGuid());
        }

        [TestMethod]
        public void Fatal()
        {
            Log.Fatal(() => "logging FATAL with lambda");
            Log.Fatal(() => "logging FATAL with lambda: {0}".FormatWith(Guid.NewGuid()));
            Log.Fatal("logging FATAL without lambda");
            Log.Fatal("logging FATAL without lambda: {0}", Guid.NewGuid());
        }
    }
}
