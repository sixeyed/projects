using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sixeyed.Mapping.Tests.Stubs
{
    [DataContract]
    public class User
    {
        [DataMember(Order=1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string FirstName { get; set; }

        [DataMember(Order = 3)]
        public string LastName { get; set; }

        [DataMember(Order = 4)]
        public DateTime DateOfBirth { get; set; }

        public string NationalInsuranceNumber { get; set; }

        public string UserName { get; set; }

        public string MemorableQuestion { get; set; }

        public DateTime JoinedDate { get; set; }

        public DateTime LastAccessDate { get; set; }

        public string EmailAddress { get; set; }

        public Address Address { get; set; }

        public User()
        {
            Address = new Address();
        }
    }
}
