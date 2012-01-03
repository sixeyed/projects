using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class FullUserToConvertedPartialUserMap : ClassMap<User, UserModel>
    {
        public FullUserToConvertedPartialUserMap()
        {
            Specify(s => s.Id, t => t.Id);
            Specify<string, string>(s => s.FirstName, t => t.FirstName, c => c.ToUpper());
            Specify<string, string>(s => s.LastName, t => t.LastName, c => c.ToLower());
        }
    }
}
