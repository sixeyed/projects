using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ClassMapPerformanceTest : TestBase
    {
        public int PerformanceIterations {get; set;}

        public ClassMapPerformanceTest()
        {
            PerformanceIterations = 1000;
        }

        [TestMethod]
        public void UserToUserModel_Manual()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var partial = new UserModel();
                partial.Id = user.Id;
                partial.FirstName = user.FirstName;
                partial.LastName = user.LastName;
                partial.Address = new PartialAddress();
                partial.Address.PostCode = user.Address.PostCode.Code;
                partial.AddressLine1 = user.Address.Line1;
                partial.DateOfBirth = user.DateOfBirth;
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void FullUserToPartialUser()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var map = new FullUserToPartialUserMap();
                var user = GetFullUser();
                var partial = map.Create(user);
                AssertEqual(user, partial);
            }
        } 

        [TestMethod]
        public void FullUserToPartialUser_Actions()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var map = new FullUserToPartialUserWithActionsMap();
                var user = GetFullUser();
                var partial = map.Create(user);
                AssertEqual(user, partial);
            }
        } 

        private static User GetFullUser()
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

        private static void AssertEqual(User user, UserModel partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.Id);
            Assert.AreEqual(user.FirstName, partial.FirstName);
            Assert.AreEqual(user.LastName, partial.LastName);
            Assert.AreEqual(user.Address.Line1, partial.AddressLine1);
            Assert.AreEqual(user.Address.PostCode.Code, partial.Address.PostCode);
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }
    }
}
