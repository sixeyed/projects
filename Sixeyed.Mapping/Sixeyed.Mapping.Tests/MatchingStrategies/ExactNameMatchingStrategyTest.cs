using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using Sixeyed.Mapping.MatchingStrategies;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    public class ExactNameMatchingStrategyTest
    {
        public string thisisadifficultidentifiertomatch { get; set; }
        public string THS_is_a_dificult_identiffier_to_mtch { get; set; }

        [TestMethod]
        public void IsMatch_Positive()
        {
            var input = "thisisadifficultidentifiertomatch";
            var property = typeof(ExactNameMatchingStrategyTest).GetProperty("thisisadifficultidentifiertomatch");
            var matching = new ExactNameMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, input));
            Assert.IsFalse(matching.IsMatch(property,input.ToUpper()));            
        }
              
        [TestMethod]
        public void GetLookup_Positive()
        {
            var input = "THS_is_a_dificult_identiffier_to_mtch";
            var expected = "THS_is_a_dificult_identiffier_to_mtch";
            var matching = new ExactNameMatchingStrategy();
            Assert.AreEqual(matching.GetLookup(expected), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Negative()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "This IS a *difficult* identifier to_match!";
            var matching = new ExactNameMatchingStrategy();
            Assert.AreNotEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreNotEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Empty()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "";
            var matching = new AggressiveNameMatchingStrategy();
            Assert.AreNotEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreNotEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }
    }
}
