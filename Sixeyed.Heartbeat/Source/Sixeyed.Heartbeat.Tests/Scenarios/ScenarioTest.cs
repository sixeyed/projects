using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Heartbeat.Tests.Scenarios.Stubs;

namespace Sixeyed.Heartbeat.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ScenarioTest
    {
        [TestMethod]
        public void DisposedHearbeat()
        {
            var service = new StubService();
            service.RunProcess();
        }
    }
}
