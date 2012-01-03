using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime LastModified { get; set; }

        public string AddressLine1 { get; set; }

        public PartialAddress Address { get; set; }

        public UserModel()
        {
            Address = new PartialAddress();
        }
    }
}
