using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;
using System.Xml;
using System.Xml.Linq;

namespace Sixeyed.Mapping.Tests.Stubs.Maps
{
    public class XPathToUserMap : XDocumentMap<User>
    {
        public XPathToUserMap()
        {
            Specify("/User/@Id", t => t.Id);
            Specify("/User/DateOfBirth", t => t.DateOfBirth);
            //etc.
            Specify("/User/EmailAddress", t => t.EmailAddress);
            Specify("/User/@FirstName", t => t.FirstName);
            Specify<string, string>("/User/@LastName", t => t.LastName, c=> c.ToUpper());
            Specify("/User/NationalInsuranceNumber", t => t.NationalInsuranceNumber);
            Specify("/User/Address/Line1", t => t.Address.Line1);
            Specify("/User/Address/Line2", t => t.Address.Line2);
            Specify("/User/Address/Postcode", t => t.Address.PostCode.Code);
        }
    }
}
