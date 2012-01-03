using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using System.Collections.Generic;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    [DeploymentItem("Stubs\\StubData.sdf")]
    public class DataReaderMapTest : DataReaderMapTestBase
    {
        public DataReaderMapTest() { }

        [TestMethod]
        public void Create()
        {
            var id = RandomGuid();
            var firstName = RandomGuidString();
            var lastName = RandomGuidString();

            var stubReader = GetStubReader(id, firstName, lastName);

            var map = new FullUserFromStubDataReaderMap();
            var user = map.Create(stubReader);

            Assert.IsNotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
        }

        [TestMethod]
        public void Create_WithConversion()
        {
            var id = RandomGuid();
            var firstName = RandomGuidString();
            var lastName = RandomGuidString();

            var stubReader = GetStubReader(id, firstName, lastName);

            var map = new FullUserFromStubDataReaderMap()
                            .Specify<string, string>("FIRSTNAME", t => t.FirstName, c => c.ToUpper());
            var user = map.Create(stubReader);

            Assert.IsNotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName.ToUpper(), user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
        }

        [TestMethod]
        public void Populate_WithSql()
        {
            User targetUser = new User();
            using (var sourceUsers = GetSqlDataReader())
            {
                sourceUsers.Read();
                new FullUserFromDataReaderMap().Populate(sourceUsers, targetUser);
            }
            AssertFirstUser(targetUser, true, true);
        }

        [TestMethod]
        public void Populate_WithSql_WithExactNameMatching()
        {
            User targetUser = new User();
            using (var sourceUsers = GetSqlDataReader())
            {
                sourceUsers.Read();
                new FullUserFromDataReaderMap()
                        .Matching<ExactNameMatchingStrategy>()
                        .Populate(sourceUsers, targetUser);
            }
            AssertFirstUser(targetUser);
        }

        [TestMethod]
        public void Populate_WithSql_WithCache()
        {
            User targetUser = new User();
            using (var sourceUsers = GetSqlDataReader())
            {
                sourceUsers.Read();
                new FullUserFromDataReaderMap()
                        .Cache<DictionaryCachingStrategy>()
                        .Populate(sourceUsers, targetUser);
            }
            AssertFirstUser(targetUser);
        }

        [TestMethod]
        public void CreateList_WithSql()
        {
            IEnumerable<User> targetUsers = null;
            using (var sourceUsers = GetSqlDataReader())
            {
                targetUsers = new FullUserFromDataReaderMap().CreateList(sourceUsers);
            }
            AssertUsers(targetUsers, true, true);
        }

        [ExpectedException(typeof(Sixeyed.Mapping.Exceptions.MappingException))]
        [TestMethod]
        public void Create_MultipleReaders()
        {
            List<User> users = null;
            using (var addressReader = GetAddressSqlDataReader())
            {
                using (var userReader = GetUserSqlDataReader())
                {
                    var map = new FullUserFromDataReaderMap();
                    map.ThrowMappingExceptions = true;
                    users = map.CreateList(userReader, addressReader);
                    //should error - multiple readers not allowed
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Create_MultipleReaders_NoException()
        {
            List<User> users = null;
            using (var addressReader = GetAddressSqlDataReader())
            {
                using (var userReader = GetUserSqlDataReader())
                {
                    var map = new FullUserFromDataReaderMap();
                    users = map.CreateList(userReader, addressReader);
                    //should return null - multiple readers not allowed
                    Assert.AreEqual(null, users);
                }
            }
        }

        //public const int PerformanceIterations = 50000;
        public const int PerformanceIterations = 1000;

        [TestMethod]
        public void Performance_AutoMap_Sql()
        {
            using (var reader = GetSqlDataReader())
            {
                reader.Read();
                var addressMap = new DataReaderAutoMap<Address>()
                                        .Specify((s, t) => t.PostCode.Code = (string)s["PostCode"]);
                var map = new DataReaderAutoMap<User>()
                                .Specify("UserId", t => t.Id)
                                .Specify((s, t) => t.Address = addressMap.Create(s));
                for (int i = 0; i < PerformanceIterations; i++)
                {
                    var user = map.Create(reader);
                }
            }
        }

        [TestMethod]
        public void Performance_Manual_Sql()
        {
            using (var reader = GetSqlDataReader())
            {
                reader.Read();
                for (int i = 0; i < PerformanceIterations; i++)
                {
                    var user = new User();
                    user.Id = (Guid)reader["UserId"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    user.Address = new Address();
                    user.Address.Line1 = (string)reader["Line1"];
                    user.Address.Line2 = (string)reader["Line2"];
                    user.Address.PostCode.Code = (string)reader["PostCode"];
                }
            }
        }        
    }
}
