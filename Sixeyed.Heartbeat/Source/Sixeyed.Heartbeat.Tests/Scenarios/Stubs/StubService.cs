using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Heartbeat.Tests.Scenarios.Stubs
{
    public class StubService
    {
        public void RunProcess()
        {
            using (var component = new StubComponent())
            {
                component.Process();
            }
        }
    }
}
