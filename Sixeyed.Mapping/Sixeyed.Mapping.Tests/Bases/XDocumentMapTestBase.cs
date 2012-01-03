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

namespace Sixeyed.Mapping.Tests
{
    public abstract class XDocumentMapTestBase : TestBase
    {
        protected static User GetFullUser()
        {
            User user = new User
            {
                Id = RandomGuid(),
                FirstName = RandomGuidString(),
                LastName = RandomGuidString(),
                DateOfBirth = RandomDate(),
                EmailAddress = RandomGuidString(),
                NationalInsuranceNumber = RandomGuidString(),
                Address = new Address
                {
                    Line1 = RandomGuidString(),
                    Line2 = RandomGuidString(),
                    PostCode = new PostCode
                    {
                        InwardCode = RandomGuidString().Substring(0, 3),
                        OutwardCode = RandomGuidString().Substring(0, 3)
                    }
                }
            };
            return user;
        }

        protected static void AssertEqual(User user, User otherUser, bool withUpperLastName = true, bool withAddress = false)
        {
            Assert.IsNotNull(otherUser);
            Assert.AreEqual(user.Id, otherUser.Id);
            Assert.AreEqual(user.FirstName, otherUser.FirstName);
            if (withUpperLastName)
            {
                Assert.AreEqual(user.LastName.ToUpper(), otherUser.LastName);
            }
            else
            {
                Assert.AreEqual(user.LastName, otherUser.LastName);
            }
            Assert.AreEqual(user.DateOfBirth, otherUser.DateOfBirth);
            Assert.AreEqual(user.EmailAddress, otherUser.EmailAddress);
            Assert.AreEqual(user.NationalInsuranceNumber, otherUser.NationalInsuranceNumber);
            if (withAddress)
            {
                Assert.AreEqual(user.Address.Line1, otherUser.Address.Line1);
                Assert.AreEqual(user.Address.Line2, otherUser.Address.Line2);
                Assert.AreEqual(user.Address.PostCode.Code, otherUser.Address.PostCode.Code);
            }
        }

        protected static string GetUserXml(string xmlFormat, User user)
        {
            return string.Format(xmlFormat, user.Id, user.FirstName, user.LastName, user.DateOfBirth, user.EmailAddress, user.NationalInsuranceNumber, user.Address.Line1, user.Address.Line2, user.Address.PostCode.Code);
        }

        public const string UserElementXmlFormat = "<User><Id>{0}</Id><FirstName>{1}</FirstName><LastName>{2}</LastName><DateOfBirth>{3}</DateOfBirth><EmailAddress>{4}</EmailAddress><NationalInsuranceNumber>{5}</NationalInsuranceNumber></User>";

        public const string UserAttributeXmlFormat = "<User Id=\"{0}\" FirstName=\"{1}\" LastName=\"{2}\" DateOfBirth=\"{3}\" EmailAddress=\"{4}\" NationalInsuranceNumber=\"{5}\"/>";

        public const string UserMixedXmlFormat = "<User Id=\"{0}\" FirstName=\"{1}\" LastName=\"{2}\"><DateOfBirth>{3}</DateOfBirth><EmailAddress>{4}</EmailAddress><NationalInsuranceNumber>{5}</NationalInsuranceNumber><Address><Line1>{6}</Line1><Line2>{7}</Line2><Postcode>{8}</Postcode></Address></User>";
    }
}