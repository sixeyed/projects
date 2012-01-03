using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class AccountToPartialAccountMap : ClassMap<Account, PartialAccount>
    {
        public AccountToPartialAccountMap()
        {
            Specify(s => s.Alias, t => t.Alias);
    Specify((s, t) => t.User = new FullUserToPartialUserMap().Create(s.User));
    //or:
    Specify(s => s.User, t => t.User, c => new FullUserToPartialUserMap().Create(c));
    //or:
    Specify((s, t) => t.User = AutoMap<User, UserModel>.CreateTarget(s.User));
        }
    }
}
