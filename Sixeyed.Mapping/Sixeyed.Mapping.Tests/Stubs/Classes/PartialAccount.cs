using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class PartialAccount
    {
        public string Alias { get; set; }

        public UserModel User {get; set;}
    }
}
