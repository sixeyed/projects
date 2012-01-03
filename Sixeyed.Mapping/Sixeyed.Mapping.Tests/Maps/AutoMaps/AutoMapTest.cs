using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.Exceptions;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AutoMapTest : TestBase
    {
        public AutoMapTest() { }

        [TestMethod]
        public void AutoMapUnspecifiedTargets()
        {
            //automap should always automap:
            var map = new AutoMap<User, UserModel>();
            map.AutoMapUnspecifiedTargets = false;
            Assert.AreEqual(true, map.AutoMapUnspecifiedTargets);
        }

        [TestMethod]
        public void AutoMap_Create_ClassSource()
        {
            var map = new AutoMap<User, UserModel>()
                            .Specify(s => s.Address.Line1, t => t.AddressLine1);
            var user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
        }

        [TestMethod]
        public void AutoMap_PopulateTarget_ClassSource()
        {            
            var user = GetFullUser();
            var partial = new UserModel();
            AutoMap<User, UserModel>.PopulateTarget(user, partial);            
            AssertEqual(user, partial, false);
        }

        [TestMethod]
        public void CreateTarget_ClassSource()
        {
            User user = GetFullUser();
            var map = new UserToUserModelMap();
            UserModel model = map.Create(user);
            AssertEqual(user, model);
        }

        [TestMethod]
        public void AutoMap_Create_ClassSource_WithCache()
        {
            AutoMap<User, UserModel> map = new AutoMap<User, UserModel>().Cache<DictionaryCachingStrategy>();
            map.Specify(s => s.Address.Line1, t => t.AddressLine1);
            User user = GetFullUser();
            UserModel partial = map.Create(user);
            AssertEqual(user, partial);
            AutoMap<User, UserModel> map2 = new AutoMap<User, UserModel>().Cache<DictionaryCachingStrategy>();
            partial = map2.Create(user);
            AssertEqual(user, partial);
        }

        [TestMethod]
        public void AutoMap_CreateList()
        {
            List<User> users = GetUsers(5);
            List<UserModel> models = AutoMap<User, UserModel>.CreateTargetList(users);
            Assert.IsNotNull(models);
            Assert.AreEqual(5, models.Count);
            for (int i = 09; i < 5; i++)
            {
                AssertEqual(users[i], models[i]);
            }

        }

        private List<User> GetUsers(int numberOfUsers)
        {
            List<User> users = new List<User>(numberOfUsers);
            for (int i = 0; i < numberOfUsers; i++)
            {
                users.Add(GetFullUser());
            }
            return users;
        }

        [TestMethod]
        public void AutoMap_Create_WithSpecify()
        {
            AutoMap<User, LegacyUser> map = 
                new AutoMap<User, LegacyUser>()
                .Matching<SimpleNameMatchingStrategy>()
                .Specify(s => s.DateOfBirth, t => t.D_O_B);
            User user = GetFullUser();
            LegacyUser partial = map.Create(user);
            AssertEqual(user, partial);
        }

        [TestMethod]
        public void AutoMap_Create_WithSpecifyAndConvert()
        {
            AutoMap<User, LegacyUser> map =
                new AutoMap<User, LegacyUser>()
                .Matching<SimpleNameMatchingStrategy>()
                .Specify<DateTime, DateTime>(s => s.DateOfBirth, t => t.D_O_B, c => c.AddDays(1));
            User user = GetFullUser();
            LegacyUser partial = map.Create(user);

            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.ID);
            Assert.AreEqual(user.FirstName, partial.First_Name);
            Assert.AreEqual(user.LastName, partial.Last_Name);
            Assert.AreEqual(user.DateOfBirth.AddDays(1), partial.D_O_B);
            Assert.AreEqual(default(DateTime), partial.Last_Modified);
        }

        [TestMethod]
        public void AutoMap_Create_WithActions()
        {
            AutoMap<User, UserModel> map =
                new AutoMap<User, UserModel>()
                .Matching<SimpleNameMatchingStrategy>()
                .Specify((s, t) => t.Address.PostCode = s.Address.PostCode.Code)
                .Specify((s, t) => t.AddressLine1 = s.Address.Line1);
            User user = GetFullUser();
            
            UserModel partial = map.Create(user);

            AssertEqual(user, partial);
            Assert.AreEqual(user.Address.Line1, partial.AddressLine1);
            Assert.AreEqual(user.Address.PostCode.Code, partial.Address.PostCode);
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }


        private static void AssertEqual(User user, UserModel partial, bool addressLine1Mapped = true)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.Id);
            Assert.AreEqual(user.FirstName, partial.FirstName);
            Assert.AreEqual(user.LastName, partial.LastName);
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
            if (addressLine1Mapped)
            {
                Assert.AreEqual(user.Address.Line1, partial.AddressLine1);
            }
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }

        private static void AssertEqual(User user, LegacyUser partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.ID);
            Assert.AreEqual(user.FirstName, partial.First_Name);
            Assert.AreEqual(user.LastName, partial.Last_Name);
            Assert.AreEqual(user.DateOfBirth, partial.D_O_B);
            Assert.AreEqual(default(DateTime), partial.Last_Modified);
        }

        private static void AssertNotEqual(User user, DifferentCaseUser partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreNotEqual(user.Id, partial.ID);
            Assert.AreNotEqual(user.FirstName, partial.Firstname);
            Assert.AreNotEqual(user.LastName, partial.Lastname);
            Assert.AreNotEqual(user.DateOfBirth, partial.Dateofbirth);
        }

        [TestMethod]
        public void AutoMap_Populate_ClassSource()
        {
            AutoMap<User, UserModel> map = new AutoMap<User, UserModel>().Matching<ExactNameMatchingStrategy>();
            map.Specify(s => s.Address.Line1, t => t.AddressLine1);
            User user = GetFullUser();
            UserModel partial = new UserModel();
            map.Populate(user, partial); 
            AssertEqual(user, partial);
        }

        [TestMethod]
        public void AutoMap_Populate_NoCaseMatch()
        {
            AutoMap<User, DifferentCaseUser> map = new AutoMap<User, DifferentCaseUser>().Matching<ExactNameMatchingStrategy>();
            User user = GetFullUser();
            DifferentCaseUser partial = new DifferentCaseUser();
            map.Populate(user, partial);
            AssertNotEqual(user, partial);
        }

        public const int PerformanceIterations = 100;

        [TestMethod]
        public void UserToUserModel_Manual()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new UserModel();
                partial.Id = user.Id;
                partial.FirstName = user.FirstName;
                partial.LastName = user.LastName;
                partial.DateOfBirth = user.DateOfBirth;
                partial.AddressLine1 = user.Address.Line1;
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>()
                                        .Specify((s, t) => t.AddressLine1 = s.Address.Line1)
                                        .Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_Cached()
        {
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = new AutoMap<User, UserModel>()
                                            .Specify((s, t) => t.AddressLine1 = s.Address.Line1)
                                            .Cache<DictionaryCachingStrategy>()
                                            .Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_Reused()
        {
            var map = new AutoMap<User, UserModel>();
            map.Specify((s, t) => t.AddressLine1 = s.Address.Line1);
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = map.Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserToUserModel_StaticMap()
        {
            var map = new UserToUserModelMap();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                User user = GetFullUser();
                UserModel partial = map.Create(user);
                AssertEqual(user, partial);
            }
        }

        [TestMethod]
        public void UserThrowingExceptions()
        {
            var user = GetFullUser();
            var map = new AutoMap<User, ExceptionThrowingUser>();
            map.ThrowMappingExceptions = true;
            AssertRaisesException<MappingException>(() => map.Create(user), "MappingStrategy.DoMap failed, target: FirstName, error: The method or operation is not implemented.");
            map.ThrowMappingExceptions = false;
            var mapped = map.Create(user);
            Assert.IsNotNull(map);
            Assert.AreEqual(user.Id, mapped.Id);
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
    }
}
