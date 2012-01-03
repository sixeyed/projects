using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests
{
    public abstract class TestBase
    {
        protected static Random Random = new Random();

        protected static string RandomGuidString()
        {
            return RandomGuid().ToString();
        }

        protected static string RandomString()
        {
            return RandomGuidString().Replace("-", "");
        }

        protected static Guid RandomGuid()
        {
            return Guid.NewGuid();
        }

        protected static DateTime RandomDate()
        {
            return new DateTime(Random.Next(1980, 2009), Random.Next(1, 12), Random.Next(1, 28));
        }

        protected static bool RandomBool()
        {
            return Random.Next() % 2 == 0;
        }

        protected static void AssertRaisesException<TException>(Action action, string expectedMessageFormat, params object[] args)
            where TException : Exception
        {
            var expectedMessage = string.Format(expectedMessageFormat, args);
            try
            {
                action();
                Assert.Fail("Call suceeded. Expected exception of type: {0} with message: {1}".FormatWith(typeof(TException).Name, expectedMessage));
            }
            catch (Exception ex)
            {
                if (ex is AssertFailedException)
                    throw ex;

                var exception = ex as TException;
                Assert.IsNotNull(exception, "Expected exception of type: {0}, actual type: {1}".FormatWith(typeof(TException).Name, ex.GetType().Name));
                Assert.AreEqual(expectedMessage, exception.Message, "Expected message: {0}".FormatWith(expectedMessage));
            }
        }
    }
}
