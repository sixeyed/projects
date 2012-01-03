using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using System.Data;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.Exceptions;
using System;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    [DeploymentItem("Stubs\\StubData.sdf")]
    public class DataReaderAutoMapTest : DataReaderMapTestBase
    {
        [TestMethod]
        public void AutoMapUnspecifiedTargets()
        {
            //automap should always automap:
            var map = new DataReaderAutoMap<User>();
            map.AutoMapUnspecifiedTargets = false;
            Assert.AreEqual(true, map.AutoMapUnspecifiedTargets);
        }

        [TestMethod]
        public void Create_WithAggressiveNameMatching()
        {
            var id = RandomGuid();
            var firstName = RandomGuidString();
            var lastName = RandomGuidString();

            var reader = GetStubReader(id, firstName, lastName);
            var user = new DataReaderAutoMap<User>().Matching<AggressiveNameMatchingStrategy>().Create(reader);

            //"Id" will not match "USERID" will not match with aggressive strategy:
            Assert.IsNotNull(user);
            Assert.AreEqual(new Guid(), user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
        }

        [TestMethod]
        public void Create_WithLegacyNameMatching()
        {
            var id = RandomGuid();
            var firstName = RandomGuidString();
            var lastName = RandomGuidString();

            var stubReader = GetLegacyStubReader(id, firstName, lastName);

            var map = new DataReaderAutoMap<User>().Matching<LegacyNameMatchingStrategy>();
            User user = map.Create(stubReader);

            Assert.IsNotNull(user);
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
        }

        [TestMethod]
        public void PopulateTarget_WithSql_Static()
        {
            User targetUser = new User();
            using (var sourceUsers = GetSqlDataReader())
            {
                sourceUsers.Read();
                DataReaderAutoMap<User>.PopulateTarget(sourceUsers, targetUser);
            }
            AssertFirstUser(targetUser);
        }

        [TestMethod]
        public void CreateTargetList_WithSql_Static()
        {
            IEnumerable<User> targetUsers = null;
            using (var sourceUsers = GetSqlDataReader())
            {
                targetUsers = DataReaderAutoMap<User>.CreateTargetList(sourceUsers);
            }            
            AssertUsers(targetUsers);
        }

        [TestMethod]
        public void CreateList_WithSql_WithCache()
        {
            IEnumerable<User> targetUsers = null;
            using (var sourceUsers = GetSqlDataReader())
            {
                var map = new DataReaderAutoMap<User>().Cache<DictionaryCachingStrategy>();
                map.ThrowMappingExceptions = true;
                targetUsers = map.CreateList(sourceUsers);
            }
            AssertUsers(targetUsers);
        }

        [TestMethod]
        public void PopulateList()
        {
            //populate clears down the list, check with a dummy entry:
            var dummyId = RandomGuid();
            var targetUsers = new List<User>();
            targetUsers.Add(new User() { Id = dummyId });
            using (var sourceUsers = GetSqlDataReader())
            {
                var map = new DataReaderAutoMap<User>();
                map.PopulateList(targetUsers, sourceUsers);
            }
            var dummy = (from t in targetUsers
                         where t.Id == dummyId
                         select t).SingleOrDefault();
            Assert.IsNull(dummy);
            AssertUsers(targetUsers);
        }

        [TestMethod]
        public void AppendList()
        {
            //append adds to the list, check with a dummy entry:
            var dummyId = RandomGuid();
            var targetUsers = new List<User>();
            targetUsers.Add(new User() { Id = dummyId });
            using (var sourceUsers = GetSqlDataReader())
            {
                var map = new DataReaderAutoMap<User>();
                map.AppendList(targetUsers, sourceUsers);
            }
            var dummy = (from t in targetUsers
                         where t.Id == dummyId
                         select t).SingleOrDefault();
            Assert.IsNotNull(dummy);
            //remove the dummy and check the mapped entires:
            targetUsers.Remove(dummy);
            AssertUsers(targetUsers);
        }   

        [TestMethod]
        public void Create_AutoMap_Sql_SpecifyAction()
        {
            User user = null;
            using (var reader = GetSqlDataReader())
            {
                reader.Read();
                var addressMap = new DataReaderAutoMap<Address>()
                                        .Specify((s, t) => t.PostCode.Code = (string)s["PostCode"]);
                var map = new DataReaderAutoMap<User>()
                                .Specify("UserId", t => t.Id)
                                .Specify((s, t) => t.Address = addressMap.Create(s));
                user = map.Create(reader);
            }
            AssertFirstUser(user, true, true);
        }

        [TestMethod]
        public void Create_AutoMap_Sql_SpecifyFunc()
        {
            User user = null;
            using (var reader = GetSqlDataReader())
            {
                reader.Read();
                var addressMap = new DataReaderAutoMap<Address>()
                                        .Specify("PostCode", t => t.PostCode.Code);
                var map = new DataReaderAutoMap<User>()
                                .Specify("UserId", t => t.Id)
                                .Specify((s, t) => t.Address = addressMap.Create(s));
                user = map.Create(reader);
            }
            AssertFirstUser(user, true, true);
        }

        [ExpectedException(typeof(MappingException))]
        [TestMethod]
        public void CreateList_MultipleReaders()
        {
            List<User> users = null;
            using (var addressReader = GetAddressSqlDataReader())
            {
                using (var userReader = GetUserSqlDataReader())
                {
                    var map = new DataReaderAutoMap<User>();
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
                    var map = new DataReaderAutoMap<User>();
                    users = map.CreateList(userReader, addressReader);
                    //should return null - multiple readers not allowed
                    Assert.AreEqual(null, users);
                }
            }
        }

        [TestMethod]
        public void Create_NestedMaps()
        {
            User user = null;
            using (var addressReader = GetAddressSqlDataReader())
            {
                addressReader.Read();
                using (var userReader = GetUserSqlDataReader())
                {
                    userReader.Read();
                    var addressMap = new DataReaderAutoMap<Address>()
                                             .Specify("PostCode", t => t.PostCode.Code);
                    var map = new DataReaderAutoMap<User>()
                                    .Specify("UserId", t => t.Id)
                                    .Specify((s, t) => t.Address = addressMap.Create(addressReader));
                    user = map.Create(userReader);
                }                
            }
            AssertFirstUser(user, true, true);
        }

        [TestMethod]
        public void Create_NestedMaps_WithConversion()
        {
            Address address = null;
            using (var addressReader = GetAddressSqlDataReader())
            {
                addressReader.Read();
                var addressMap = new DataReaderAutoMap<Address>()
                                         .Specify<string, string>("PostCode", t => t.PostCode.Code, c => c.ToLower());
                address = addressMap.Create(addressReader);
            }
            using (var sourceUsers = GetSqlDataReader())
            {
                Assert.IsTrue(sourceUsers.Read());
                Assert.IsNotNull(address);
                Assert.IsNotNull(address.PostCode);
                Assert.AreEqual((string)sourceUsers["PostCode"].ToString().ToLower(), address.PostCode.OriginalCode);
            }
        }
    }
}
