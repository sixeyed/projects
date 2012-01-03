using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.MatchingStrategies;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ClassMapTest : ClassMapTestBase
    {
        [TestMethod]
        public void FullUserToPartialUser()
        {
            FullUserToPartialUserMap map = new FullUserToPartialUserMap();
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
        }

        [TestMethod]
        public void FullUserToPartialUser_Converted()
        {
            FullUserToConvertedPartialUserMap map = new FullUserToConvertedPartialUserMap();
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            Assert.AreEqual(user.Id, partial.Id);
            Assert.AreEqual(user.FirstName.ToUpper(), partial.FirstName);
            Assert.AreEqual(user.LastName.ToLower(), partial.LastName);
            //date of birth is not specified, and should not be auto-mapped:
            Assert.AreEqual(default(DateTime), partial.DateOfBirth);
        }

        [TestMethod]
        public void FullUserToPartialUser_Funcs()
        {
            FullUserToPartialUserWithFuncsMap map = new FullUserToPartialUserWithFuncsMap();
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
            Assert.AreEqual(user.Address.Line1, partial.AddressLine1);
            Assert.AreEqual(user.Address.PostCode.Code, partial.Address.PostCode);
        }

        [TestMethod]
        public void FullUserToPartialUser_Actions()
        {
            FullUserToPartialUserWithActionsMap map = new FullUserToPartialUserWithActionsMap();
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
            Assert.AreEqual(user.Address.Line1, partial.AddressLine1);
            Assert.AreEqual(user.Address.PostCode.Code, partial.Address.PostCode);
        } 

        [TestMethod]
        public void FullUserToPartialUser_WithAutoMap()
        {
            FullUserToPartialUserMap map = new FullUserToPartialUserMap();
            map.AutoMapUnspecifiedTargets = true;
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
            //date of birth is not specified, but should  be auto-mapped:
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
        }

        [TestMethod]
        public void AccountToPartial()
        {
            Account account = new Account();
            account.Id = RandomGuid();
            account.Alias = RandomGuidString();
            account.CreatedDate = RandomDate();
            account.User = GetFullUser();
            
            AccountToPartialAccountMap map = new AccountToPartialAccountMap();
            PartialAccount partial = map.Create(account);

            Assert.IsNotNull(partial);
            Assert.AreEqual(account.Alias, partial.Alias);
            AssertEqual(account.User, partial.User);
        }

        [TestMethod]
        public void FullUserToPartialUser_WithAutoMap_WithCache()
        {
            FullUserToPartialUserMap map = new FullUserToPartialUserMap();
            map.AutoMapUnspecifiedTargets = true;
            map.Cache<MemoryCacheCachingStrategy>();
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
            //date of birth is not specified, but should  be auto-mapped:
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
        }

        [TestMethod]
        public void FullUserToDifferentCaseUser_WithAutoMap_WithMatching()
        {
            var map = new FullUserToDifferentCaseUserMap();
            map.AutoMapUnspecifiedTargets = true;
            map.Matching<AggressiveNameMatchingStrategy>();
            var user = GetFullUser();
            var partial = map.Create(user);
            AssertEqual(user, partial);
        }
    }
}
