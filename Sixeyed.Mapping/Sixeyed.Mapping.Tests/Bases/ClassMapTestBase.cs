using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.Mapping.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.Mapping.Tests
{
    public abstract class ClassMapTestBase : TestBase
    {
        protected static void AssertEqual(User user, UserModel partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.Id);
            Assert.AreEqual(user.FirstName, partial.FirstName);
            Assert.AreEqual(user.LastName, partial.LastName);
            Assert.AreEqual(default(DateTime), partial.LastModified);
        }

        protected static void AssertEqual(User user, DifferentCaseUser partial)
        {
            Assert.IsNotNull(partial);
            Assert.AreEqual(user.Id, partial.ID);
            Assert.AreEqual(user.DateOfBirth, partial.Dateofbirth);
            Assert.AreEqual(user.FirstName, partial.Firstname);
            Assert.AreEqual(user.LastName, partial.Lastname);
            Assert.AreEqual(default(DateTime), partial.Lastmodified);
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
