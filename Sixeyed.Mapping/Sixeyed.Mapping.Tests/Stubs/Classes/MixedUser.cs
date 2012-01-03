using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class LegacyUser
    {
        public Guid ID { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public DateTime D_O_B { get; set; }

        public DateTime Last_Modified { get; set; }
    }
}
