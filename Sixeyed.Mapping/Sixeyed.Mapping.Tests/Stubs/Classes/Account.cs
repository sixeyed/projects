using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class Account
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Alias { get; set; }

        public User User {get; set;}
    }
}
