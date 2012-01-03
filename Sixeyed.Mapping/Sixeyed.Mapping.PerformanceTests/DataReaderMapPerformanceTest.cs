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
    [TestClass]
    [DeploymentItem("Stubs\\StubData-LARGE.sdf")]
    public class DataReaderMapPerformanceTest : DataReaderMapTestBase
    {
        public int PerformanceIterations {get; set;}

        public DataReaderMapPerformanceTest()
        {
            PerformanceIterations = 1;
        }

        [TestMethod]
        public void FromSqlCe_Manual()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            using (var reader = GetSqlDataReader())
            {
                while (reader.Read() && count < PerformanceIterations)
                {
                    count++;
                    var user = new User();
                    user.Id = (Guid)reader["UserId"];
                    user.FirstName = (string)reader["FirstName"];
                    user.LastName = (string)reader["LastName"];
                    var address = new Address();
                    address.Line1 = (string)reader["Line1"];
                    address.Line2 = (string)reader["Line2"];
                    address.PostCode = new PostCode();
                    address.PostCode.Code = (string)reader["PostCode"];
                    user.Address = address;
                    AssertUser(reader, user, true, true);
                }
            }
            Debug.WriteLine("FromSqlCe_Manual -Mapped: {0} objects and asserted 0 failures in: {1}ms", count, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromSqlCe_AutoMap()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            var userMap = new DataReaderAutoMap<User>()
                            .Specify("UserId", t => t.Id)
                            .Specify("Line1", t => t.Address.Line1)
                            .Specify("Line2", t => t.Address.Line2)
                            .Specify("PostCode", t => t.Address.PostCode.Code);

            using (var reader = GetSqlDataReader())
            {
                while (reader.Read() && count < PerformanceIterations)
                {
                    count++;
                    var user = userMap.Create(reader);
                    AssertUser(reader, user, true, true);
                }
            }
            Debug.WriteLine("FromSqlCe_AutoMap -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromSqlCe_AutoMap_Nested()
        { 
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            var postcodeMap = new DataReaderAutoMap<PostCode>()
                                        .Specify("PostCode", t=>t.Code);
            var addressMap = new DataReaderAutoMap<Address>()
                                    .Specify((r,t)=>t.PostCode = postcodeMap.Create(r));
            var userMap = new DataReaderAutoMap<User>()
                            .Specify("UserId", t=>t.Id)
                            .Specify((r, t) => t.Address = addressMap.Create(r));
            
            using (var reader = GetSqlDataReader())
            {
                while (reader.Read() && count < PerformanceIterations)
                {
                    count++;
                    var user = userMap.Create(reader);
                    AssertUser(reader, user, true, true);
                }
            }
            Debug.WriteLine("FromSqlCe_AutoMap_Nested -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromSqlCe_StaticMap()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            var userMap = new FullUserFromDataReaderMap();
            using (var reader = GetSqlDataReader())
            {
                while (reader.Read() && count < PerformanceIterations)
                {
                    count++;
                    var user = userMap.Create(reader);
                    AssertUser(reader, user, true, true);
                }
            }
            Debug.WriteLine("FromSqlCe_StaticMap -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        [Ignore] //use only to populate database for large result sets; 250K records = 80+Mb
        public void PopulateDatabase()
        {
            var cnn = GetConnection();
            var cmd = cnn.CreateCommand();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                InsertUserWithAddress(cmd);
            }
        }

        private static void InsertUserWithAddress(System.Data.IDbCommand cmd)
        {
            var addressId = RandomGuid();
            var line1 = "line1 " + RandomString().Substring(0, 5);
            var line2 = "line2 " + RandomString().Substring(0, 8);
            var postcode = "PO" + Random.Next(1, 99) + " " + Random.Next(1, 99) + "CD";
            var sql = string.Format("INSERT INTO Addresses(AddressId, Line1, Line2, Postcode) VALUES('{0}', '{1}', '{2}', '{3}')",
                                addressId, line1, line2, postcode);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            var userId = RandomGuid();
            var firstName = "F" + RandomString().Substring(0, 10);
            var lastName = "L " + RandomString().Substring(0, 8);
            sql = string.Format("INSERT INTO Users(UserId, FirstName, LastName, AddressId) VALUES('{0}', '{1}', '{2}', '{3}')",
                                userId, firstName, lastName, addressId);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}
