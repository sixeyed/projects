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
    public class SimpleNameMatchingStrategyTest
    {
        public string thisisadifficultidentifiertomatch { get; set; }
        public string THS_is_a_dificult_identiffier_to_mtch { get; set; }

        [TestMethod]
        public void IsMatch_Positive()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var property = typeof(SimpleNameMatchingStrategyTest).GetProperty("thisisadifficultidentifiertomatch");
            var matching = new SimpleNameMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, input));
            Assert.IsTrue(matching.IsMatch(property,input.ToUpper()));            
        }

        [TestMethod]
        public void IsMatch_Positive_Complex()
        {
            var input = " Ths IS a *dificult* identiffier to_mtch! ";
            var property = typeof(SimpleNameMatchingStrategyTest).GetProperty("THS_is_a_dificult_identiffier_to_mtch");
            var matching = new SimpleNameMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, input));
            Assert.IsTrue(matching.IsMatch(property, input.ToUpper())); 
        }

        [TestMethod]
        public void GetLookup_Positive()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected ="thisisadifficultidentifiertomatch";
            var matching = new SimpleNameMatchingStrategy();
            Assert.AreEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Positive_Complex()
        {
            var input = " Ths IS a *difficult* identifier to_match! ";
            var expected = "Ths IS a *difficult* identifier to_match";
            var matching = new SimpleNameMatchingStrategy();
            Assert.AreEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Negative()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "totally different";
            var matching = new SimpleNameMatchingStrategy();
            Assert.AreNotEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreNotEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }

        [TestMethod]
        public void GetLookup_Empty()
        {
            var input = " This IS a *difficult* identifier to_match! ";
            var expected = "";
            var matching = new SimpleNameMatchingStrategy();
            Assert.AreNotEqual(matching.GetLookup(expected), matching.GetLookup(input));
            Assert.AreNotEqual(matching.GetLookup(expected.ToUpper()), matching.GetLookup(input));
        }
    }
}
