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
    public class AggressiveNameMatchingStrategyTest
    {
        public string thisisadifficultidentifiertomatch { get; set; }
        public string THS_is_a_dificult_identiffier_to_mtch { get; set; }

        [TestMethod]
        public void IsMatch_Positive()
        {
            var input = " This ES a *diffocult* identifier to_match! ";
            var property = typeof(AggressiveNameMatchingStrategyTest).GetProperty("thisisadifficultidentifiertomatch");
            var matching = new AggressiveNameMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, input));
            Assert.IsTrue(matching.IsMatch(property,input.ToUpper()));            
        }

        [TestMethod]
        public void IsMatch_Positive_Complex()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var property = typeof(AggressiveNameMatchingStrategyTest).GetProperty("THS_is_a_dificult_identiffier_to_mtch");
            var matching = new AggressiveNameMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, input));
            Assert.IsTrue(matching.IsMatch(property, input.ToUpper())); 
        }

        [TestMethod]
        public void GetLookup_Positive()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected ="thisisadifficultidentifiertomatch";
            var matching = new AggressiveNameMatchingStrategy();
            Assert.AreEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Positive_Complex()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "THS_is_a_dificult_identiffier_to_mtch";
            var matching = new AggressiveNameMatchingStrategy();
            Assert.AreEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Negative()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "totally different";
            var matching = new AggressiveNameMatchingStrategy();
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
