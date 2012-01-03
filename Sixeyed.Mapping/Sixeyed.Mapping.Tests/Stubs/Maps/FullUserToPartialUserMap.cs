using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserToPartialUserMap : ClassMap<User, UserModel>
    {
        public FullUserToPartialUserMap()
        {
            Specify(s => s.Id, t => t.Id);
            Specify(s => s.FirstName, t => t.FirstName);
            Specify(s => s.LastName, t => t.LastName);
            Specify(s => s.Address.Line1, t => t.AddressLine1);
            Specify(s => s.Address.PostCode.Code, t => t.Address.PostCode);
            Specify(s => s.DateOfBirth, t => t.DateOfBirth);
        }
    }
}
