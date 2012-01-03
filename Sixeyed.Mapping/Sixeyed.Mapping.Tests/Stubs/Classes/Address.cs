using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class Address
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public PostCode PostCode { get; set; }

        public Address()
        {
            PostCode = new PostCode();
        }
    }
}
