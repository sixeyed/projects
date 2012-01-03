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
    public class XDocumentToUserMap : XDocumentMap<User>
    {
        public XDocumentToUserMap()
        {
            Specify(XmlNodeType.Element, "Id", t => t.Id);
            Specify(XmlNodeType.Element, "DateOfBirth", t => t.DateOfBirth);
            //etc.
            Specify(XmlNodeType.Element, "EmailAddress", t => t.EmailAddress);
            Specify(XmlNodeType.Element, "FirstName", t => t.FirstName);
            Specify<string, string>(XmlNodeType.Element, "LastName", t => t.LastName, c => c.ToUpper());
            Specify(XmlNodeType.Element, "NationalInsuranceNumber", t => t.NationalInsuranceNumber);
        }
    }
}
