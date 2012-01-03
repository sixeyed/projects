using System;
using System.Diagnostics;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    public class AutoMapperComparisonTest : TestBase
    {
        public int PerformanceIterations {get; set;}

        public AutoMapperComparisonTest()
        {
            PerformanceIterations = 1000;
        }

        [TestMethod]
        public void Comparison()
        {
            UserToUserModel_Manual();
            UserToUserModel_AutoMap();
            UserToUserModel_AutoMapper();
        }

        [TestMethod]
        public void Comparison_WithAutoMapCache()
        {
            UserToUserModel_Manual();
            UserToUserModel_AutoMap_WithCache();
            UserToUserModel_AutoMapper();
        }

        [TestMethod]
        public void UserToUserModel_Manual()
        {
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var partial = new UserModel();
                partial.Id = user.Id;
                partial.FirstName = user.FirstName;
                partial.LastName = user.LastName;
                partial.DateOfBirth = user.DateOfBirth;
                AssertEqual(user, partial);
            }
            Debug.WriteLine("UserToUserModel_Manual -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UserToUserModel_AutoMap()
        {
            var stopwatch = Stopwatch.StartNew();
            var map = new AutoMap<User, UserModel>();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var partial = map.Create(user);
                AssertEqual(user, partial);
            }
            Debug.WriteLine("UserToUserModel_AutoMap -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UserToUserModel_AutoMap_WithCache()
        {
            var stopwatch = Stopwatch.StartNew();
            var map = new AutoMap<User, UserModel>().Cache<DictionaryCachingStrategy>();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var partial = map.Create(user);
                AssertEqual(user, partial);
            }
            Debug.WriteLine("UserToUserModel_AutoMap_WithCache -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void UserToUserModel_AutoMapper()
        {
            var stopwatch = Stopwatch.StartNew();
            Mapper.CreateMap<User, UserModel>()
                  .ForMember(x => x.Address, opt => opt.Ignore());
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var partial = Mapper.Map<User, UserModel>(user);
                AssertEqual(user, partial);
            }
            Debug.WriteLine("UserToUserModel_AutoMapper -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
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
            Assert.AreEqual(user.DateOfBirth, partial.DateOfBirth);
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }
    }
}
