using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserToPartialUserWithFuncsMap : ClassMap<User, UserModel>
    {
        public FullUserToPartialUserWithFuncsMap()
        {
            Specify(s => s.Id, t => t.Id);
            Specify(s => s.FirstName, t => t.FirstName);
            Specify(s => s.LastName, t => t.LastName);
            Specify<string, string>(s => s.Address.Line1, t => t.AddressLine1);
            Specify<string, string>(s => s.Address.PostCode.Code, t => t.Address.PostCode);
        }
    }
}
