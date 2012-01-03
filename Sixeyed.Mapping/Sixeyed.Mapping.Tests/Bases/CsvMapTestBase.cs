using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.Tests.Stubs.Strategies;

namespace Sixeyed.Mapping.Tests
{
    public abstract class CsvMapTestBase : TestBase
    {
        protected string GetCsvRow(User user, char delimiter = ',')
        {
            return string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}{1}{6}",
                user.Id, delimiter, user.FirstName, user.LastName, user.DateOfBirth, user.Address.Line1, user.Address.Line2);
        }

        protected string GetPropertyNameOrderedRow(User user, char delimiter = ',')
        {
            return string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}",
                user.Id, delimiter, user.FirstName, user.LastName, user.DateOfBirth, user.NationalInsuranceNumber);
        }

        protected string GetPropertyNameOrderedRowWithAddress(User user, char delimiter = ',')
        {
            return string.Format("{0}{1}{2}{1}{3}{1}{4}{1}{5}",
                user.Id, delimiter, user.FirstName, user.LastName, user.DateOfBirth, user.NationalInsuranceNumber, user.Address.Line1, user.Address.Line2);
        }
        
        protected static void AssertEqual(User original, User mapped)
        {
            Assert.IsNotNull(mapped);
            Assert.AreEqual(original.Id, mapped.Id);
            Assert.AreEqual(original.FirstName, mapped.FirstName);
            Assert.AreEqual(original.LastName, mapped.LastName);
            Assert.AreEqual(original.DateOfBirth, mapped.DateOfBirth);
        }

        protected static void AssertEqualByPropertyOrder(User original, User mapped, bool withAddress = false)
        {
            Assert.IsNotNull(mapped);
            Assert.AreEqual(original.Id, mapped.Id);
            Assert.AreEqual(original.FirstName, mapped.FirstName);
            Assert.AreEqual(original.LastName, mapped.LastName);
            Assert.AreEqual(original.DateOfBirth, mapped.DateOfBirth);
            Assert.AreEqual(original.NationalInsuranceNumber, mapped.NationalInsuranceNumber);
            if (withAddress)
            {
                Assert.IsNotNull(mapped.Address);
                Assert.AreEqual(original.Address.Line1, mapped.Address.Line1);
                Assert.AreEqual(original.Address.Line2, mapped.Address.Line2);
            }
        }

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
    }
}
