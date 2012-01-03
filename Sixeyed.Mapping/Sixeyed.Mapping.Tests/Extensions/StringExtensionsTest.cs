using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Tests
{

    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void RemoveVowels()
        {
            var input = "the quick brown fox jumped over the lazy dog";
            var expected = "th qck brwn fx jmpd vr th lzy dg";
            Assert.AreEqual(expected, input.RemoveVowels());
            Assert.AreEqual(expected.ToUpper(), input.ToUpper().RemoveVowels());
        }

        [TestMethod]
        public void RemoveDuplicateCharacters()
        {
            var input = "llareggub";
            var expected = "laregub";
            Assert.AreEqual(expected, input.RemoveDuplicateCharacters());
            Assert.AreEqual(expected.ToUpper(), input.ToUpper().RemoveDuplicateCharacters());
            input = "lareeeeggubbb";
            Assert.AreEqual(expected, input.RemoveDuplicateCharacters());
            Assert.AreEqual(expected.ToUpper(), input.ToUpper().RemoveDuplicateCharacters());
            input = "llaarreeggubb";
            Assert.AreEqual(expected, input.RemoveDuplicateCharacters());
            Assert.AreEqual(expected.ToUpper(), input.ToUpper().RemoveDuplicateCharacters());
        }

        [TestMethod]
        public void RemoveIllegalPropertyNameCharacters()
        {
            var input = " this is *not* a legal identifier!";
            var expected = "thisisnotalegalidentifier";
            Assert.AreEqual(expected, input.RemoveIllegalPropertyNameCharacters());
            Assert.AreEqual(expected.ToUpper(), input.ToUpper().RemoveIllegalPropertyNameCharacters());
        }
    }
}
