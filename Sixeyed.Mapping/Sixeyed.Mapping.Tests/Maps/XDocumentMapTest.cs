using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Rhino.Mocks;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Exceptions;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class XDocumentMapTest : XDocumentMapTestBase
    {
        [TestMethod]
        public void Create_Elements()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserElementXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentToUserMap();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Create_Elements_WithCache()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserElementXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentToUserMap().Cache<DictionaryCachingStrategy>();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Create_Attributes()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserAttributeXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XAttributeToUserMap();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Create_Attributes_WithMatching()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserAttributeXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XAttributeToUserMap().Matching<ExactNameMatchingStrategy>();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Create_XPath()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XPathToUserMap();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Specify_Invalid()
        {
            var map = new XPathToUserMap();
            AssertRaisesException<ArgumentException>(()=>map.Specify(XmlNodeType.CDATA, XName.Get("id", "ns"), x => x.Id), "XmlNodeType must be Element or Attribute; CDATA not supported{0}Parameter name: nodeType", Environment.NewLine);
        }
    }
}