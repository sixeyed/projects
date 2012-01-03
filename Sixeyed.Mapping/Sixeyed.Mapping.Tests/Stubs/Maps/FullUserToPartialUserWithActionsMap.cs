using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserToPartialUserWithActionsMap : ClassMap<User, UserModel>
    {
        public FullUserToPartialUserWithActionsMap()
        {
            Specify((s, t) => t.Id = s.Id);
            Specify((s, t) => t.FirstName = s.FirstName);
            Specify((s, t) => t.LastName = s.LastName);
            Specify((s, t) => t.AddressLine1 = s.Address.Line1);
            Specify((s, t) => t.Address.PostCode = s.Address.PostCode.Code);
            Specify((s, t) => t.DateOfBirth = s.DateOfBirth);
        }
    }
}
