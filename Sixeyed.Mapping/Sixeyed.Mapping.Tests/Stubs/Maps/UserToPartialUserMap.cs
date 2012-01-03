using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class UserToUserModelMap : ClassMap<User, UserModel>
    {
        public UserToUserModelMap()
        {
            Specify(s => s.Id, t => t.Id);
            Specify(s => s.FirstName, t => t.FirstName);
            Specify(s => s.LastName, t => t.LastName);
            Specify(s => s.DateOfBirth, t => t.DateOfBirth);
            Specify((s, t) => t.AddressLine1 = s.Address.Line1);
        }
    }
}
