using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using Rhino.Mocks;
using System.Collections.Generic;
using Sixeyed.Mapping.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.Mapping.Tests
{
    public abstract class DataReaderMapTestBase : TestBase
    {
        protected static void AssertUsers(IEnumerable<User> targetUsers, bool withAddress = false, bool withPostcode = false)
        {
            var index = 0;
            using (var sourceUsers = GetSqlDataReader())
            {
                while (sourceUsers.Read())
                {
                    var user = targetUsers.ElementAt(index);
                    AssertUser(sourceUsers, user, withAddress, withPostcode);
                    index++;
                }
                Assert.AreEqual(index, targetUsers.Count());
            }
        }

        protected static void AssertFirstUser(User user, bool withAddress = false, bool withPostcode = false)
        {
            using (var sourceUsers = GetSqlDataReader())
            {
                Assert.IsTrue(sourceUsers.Read());
                AssertUser(sourceUsers, user, withAddress, withPostcode);
            }
        }

        protected static void AssertUser(IDataReader sourceUsers, User user, bool withAddress = false, bool withPostcode = false)
        {
            Assert.IsNotNull(user);
            Assert.AreEqual((string)sourceUsers["FirstName"], user.FirstName);
            Assert.AreEqual((string)sourceUsers["LastName"], user.LastName);
            if (withAddress)
            {
                Assert.IsNotNull(user.Address);
                Assert.AreEqual((string)sourceUsers["Line1"], user.Address.Line1);
                Assert.AreEqual((string)sourceUsers["Line2"], user.Address.Line2);
                if (withPostcode)
                {
                    Assert.IsNotNull(user.Address.PostCode);
                    Assert.AreEqual((string)sourceUsers["PostCode"], user.Address.PostCode.Code);
                }
            }
        }

        protected DateTime FromLegacyDate(DateTime sd)
        {
            throw new NotImplementedException();
        }

        protected static IDataReader GetSqlDataReader()
        {
            var cnn = GetConnection();
            var cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"select *
                                from users u
                                join addresses a
                                on a.addressid = u.addressid;";
            var reader = cmd.ExecuteReader();
            return reader;
        }

        protected static IDbConnection _connection;

        protected static IDbConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlCeConnection(ConfigurationManager.ConnectionStrings["StubData"].ConnectionString);
            }
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }

        protected static IDataReader GetUserSqlDataReader()
        {
            var cnn = GetConnection();
            var cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"select * from users";
            var reader = cmd.ExecuteReader();
            return reader;
        }

        protected static IDataReader GetAddressSqlDataReader()
        {
            var cnn = GetConnection();
            var cmd = cnn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = @"select * from addresses";
            var reader = cmd.ExecuteReader();
            return reader;
        }

        protected static IDataReader GetStubReader(Guid id, string firstName, string lastName)
        {
            var stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(4);
            stubReader.Stub(x => x.GetName(0)).Return("USERID");
            stubReader.Stub(x => x.GetName(1)).Return("FIRSTNAME");
            stubReader.Stub(x => x.GetName(2)).Return("LASTNAME");
            stubReader.Stub(x => x.GetName(3)).Return("DUMMY");
            stubReader.Stub(x => x.GetOrdinal("USERID")).Return(0);
            stubReader.Stub(x => x.GetOrdinal("FIRSTNAME")).Return(1);
            stubReader.Stub(x => x.GetOrdinal("LASTNAME")).Return(2);
            stubReader.Stub(x => x.GetOrdinal("DUMMY")).Return(3);
            stubReader.Stub(x => x.GetOrdinal(string.Empty)).IgnoreArguments().Return(-1);
            stubReader.Stub(x => x.GetValue(0)).Return(id);
            stubReader.Stub(x => x.GetValue(1)).Return(firstName);
            stubReader.Stub(x => x.GetValue(2)).Return(lastName);
            stubReader.Stub(x => x.GetValue(3)).Return(null);
            return stubReader;
        }

        protected static IDataReader GetLegacyStubReader(Guid id, string firstName, string lastName)
        {
            var stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(4);
            stubReader.Stub(x => x.GetName(0)).Return("guid_ID");
            stubReader.Stub(x => x.GetName(1)).Return("char_FIRSTNAME");
            stubReader.Stub(x => x.GetName(2)).Return("char_LASTNAME");
            stubReader.Stub(x => x.GetName(3)).Return("bit_DUMMY");
            stubReader.Stub(x => x.GetOrdinal("guid_ID")).Return(0);
            stubReader.Stub(x => x.GetOrdinal("char_FIRSTNAME")).Return(1);
            stubReader.Stub(x => x.GetOrdinal("char_LASTNAME")).Return(2);
            stubReader.Stub(x => x.GetOrdinal("bit_DUMMY")).Return(3);
            stubReader.Stub(x => x.GetOrdinal(string.Empty)).IgnoreArguments().Return(-1);
            stubReader.Stub(x => x.GetValue(0)).Return(id);
            stubReader.Stub(x => x.GetValue(1)).Return(firstName);
            stubReader.Stub(x => x.GetValue(2)).Return(lastName);
            stubReader.Stub(x => x.GetValue(3)).Return(null);
            return stubReader;
        }
    }
}
