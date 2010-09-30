using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Heartbeat.Entities;
using Sixeyed.Heartbeat.Entities.Enums;

namespace Sixeyed.Heartbeat.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class HeartbeatLogTest
    {
        [TestMethod]
        public void Status()
        {
            var log = new HeartbeatLog();
            log.StatusCode = "START";
            Assert.AreEqual(HeartbeatStatus.Started, log.Status);
            log.Status = HeartbeatStatus.InProgress;
            Assert.AreEqual("WORKING", log.StatusCode);
        }
    }
}
