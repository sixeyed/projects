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
    public class XAttributeToUserMap : XDocumentMap<User>
    {
        public XAttributeToUserMap()
        {
            Specify(XmlNodeType.Attribute, "Id", t => t.Id);
            Specify(XmlNodeType.Attribute, "DateOfBirth", t => t.DateOfBirth);
            Specify(XmlNodeType.Attribute, "EmailAddress", t => t.EmailAddress);
            Specify(XmlNodeType.Attribute, "FirstName", t => t.FirstName);
            Specify<string, string>(XmlNodeType.Attribute, "LastName", t => t.LastName, c => c.ToUpper());
            Specify(XmlNodeType.Attribute, "NationalInsuranceNumber", t => t.NationalInsuranceNumber);
        }
    }
}
