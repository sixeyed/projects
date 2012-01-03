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
using System.Collections.Generic;
using Sixeyed.Mapping.Exceptions;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class XDocumentAutoMapTest : XDocumentMapTestBase
    {
        [TestMethod]
        public void AutoMapUnspecifiedTargets()
        {
            //automap should always automap:
            var map = new XDocumentAutoMap<User>();
            map.AutoMapUnspecifiedTargets = false;
            Assert.AreEqual(true, map.AutoMapUnspecifiedTargets);
        }

        [TestMethod]
        public void Create_Elements_Static()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserElementXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var mappedUser = XDocumentAutoMap<User>.CreateTarget(source);
            AssertEqual(user, mappedUser, false);
        }

        [TestMethod]
        public void Create_Elements_WithSpecify_Action()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
    var postcodeMap = new XDocumentAutoMap<PostCode>();
    postcodeMap.Specify(XmlNodeType.Element, "Postcode", t => t.Code);
    var addressMap = new XDocumentAutoMap<Address>();
    addressMap.Specify((s, t) => t.PostCode = postcodeMap.Create(source));
    var userMap = new XDocumentAutoMap<User>();
    userMap.Specify((s, t) => t.Address = addressMap.Create(source));
    var mappedUser = userMap.Create(source);
            AssertEqual(user, mappedUser, false, true);
        }

        [TestMethod]
        public void Create_Elements_WithConversion()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserElementXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentAutoMap<User>()
                            .Specify(XmlNodeType.Element, "FirstName", t => t.FirstName)
                            .Specify<string, string>(XmlNodeType.Element, "LastName", t => t.LastName, c => c.ToUpper());
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser);
        }

        [TestMethod]
        public void Create_Mixed_Nested_WithConversion()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentAutoMap<User>()
                            .Specify<string, string>("/User/@LastName", t => t.LastName, c => c.ToUpper())
                            .Specify("/User/Address/Line1", t => t.Address.Line1)
                            .Specify("/User/Address/Line2", t => t.Address.Line2)
                            .Specify("/User/Address/Postcode", t => t.Address.PostCode.Code);
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser, withAddress:true);
        }

        [TestMethod]
        public void CreateTarget_Attributes()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserAttributeXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var mappedUser = XDocumentAutoMap<User>.CreateTarget(source);
            AssertEqual(user, mappedUser, false);
        }

        [TestMethod]
        public void Create_Attributes_Static_WithCache()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserAttributeXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentAutoMap<User>().Cache<DictionaryCachingStrategy>();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser, false);
        }

        [TestMethod]
        public void PopulateTarget_Mixed()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var mappedUser = new User();
            XDocumentAutoMap<User>.PopulateTarget(source, mappedUser);
            AssertEqual(user, mappedUser, false);
        }

        [TestMethod]
        public void Create_Mixed_WithMatching()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentAutoMap<User>().Matching<ExactNameMatchingStrategy>();
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser, false);
        }

        [TestMethod]
        public void Create_Mixed_SpecifyNested()
        {
            var user = GetFullUser();
            var userXml = GetUserXml(UserMixedXmlFormat, user);
            var source = XDocument.Parse(userXml);
            var map = new XDocumentAutoMap<User>()
                            .Specify("/User/Address/Line1", t => t.Address.Line1)
                            .Specify("/User/Address/Line2", t => t.Address.Line2)
                            .Specify("/User/Address/Postcode", t => t.Address.PostCode.Code);
            var mappedUser = map.Create(source);
            AssertEqual(user, mappedUser, false, true);
        }

        [TestMethod]
        public void CreateTargetList_Mixed_Static()
        {
            var listSize = Random.Next(10, 20);
            var users = new List<User>(listSize);
            var root = new XElement("Users");
            for (int i = 0; i < listSize; i++)
            {
                var user = GetFullUser();
                users.Add(user);
                var userXml = GetUserXml(UserMixedXmlFormat, user);
                root.Add(XDocument.Parse(userXml).Root);
            }
            var source = new XDocument();
            source.Add(root);
            var mappedUsers = XDocumentAutoMap<User>.CreateTargetList(source);            
            Assert.AreEqual(users.Count, mappedUsers.Count);
            for (int i = 0; i < listSize; i++)
            {
                var user = users[i];
                var mappedUser = mappedUsers[i];
                AssertEqual(user, mappedUser, false);
            }           
        }

        [ExpectedException(typeof(MappingException))]
        [TestMethod]
        public void CreateList_MultipleDocuments()
        {
            List<User> users = null;
            var doc1 = new XDocument();
            var doc2 = new XDocument();
            var map = new XDocumentAutoMap<User>();
            map.ThrowMappingExceptions = true;
            users = map.CreateList(doc1, doc2);
            //should error - multiple readers not allowed
            Assert.Fail();
        }
    }
}