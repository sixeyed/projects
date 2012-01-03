using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserToDifferentCaseUserMap : ClassMap<User, DifferentCaseUser>
    {
        public FullUserToDifferentCaseUserMap()
        {
            Specify(s => s.Id, t => t.ID);
            //partial map to verify matching for automap
        }
    }
}
