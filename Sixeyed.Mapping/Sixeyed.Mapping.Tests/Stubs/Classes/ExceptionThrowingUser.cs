using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sixeyed.Mapping.Tests.Stubs
{
    [DataContract]
    public class ExceptionThrowingUser
    {
        [DataMember(Order=1)]
        public Guid Id { get; set; }

        [DataMember(Order = 2)]
        public string FirstName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}
