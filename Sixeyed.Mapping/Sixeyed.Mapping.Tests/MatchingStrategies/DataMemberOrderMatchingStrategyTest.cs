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
using System.Runtime.Serialization;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    public class DataMemberOrderMatchingStrategyTest
    {
        [DataContract]
        public class Stub
        {
            [DataMember(Order=1)]
            public int Member1 { get; set; }

            public string NonMember { get; set; }

            [DataMember]
            public int MemberNoOrder { get; set; }

            [DataMember(Order=2)]
            public string Member2 { get; set; }

            public string NonMember2 { get; set; }
        }

        [TestMethod]
        public void IsMatch_Positive()
        {            
            var property = typeof(Stub).GetProperty("Member1");
            var matching = new DataMemberOrderMatchingStrategy();
            Assert.IsTrue(matching.IsMatch(property, 1));
            property = typeof(Stub).GetProperty("Member2");
            Assert.IsTrue(matching.IsMatch(property, 2));
        }

        [TestMethod]
        public void IsMatch_Negative()
        {
            var property = typeof(Stub).GetProperty("MemberNoOrder");
            var matching = new DataMemberOrderMatchingStrategy();
            Assert.IsFalse(matching.IsMatch(property, 1));
            property = typeof(Stub).GetProperty("NonMember");
            Assert.IsFalse(matching.IsMatch(property, 2));
            property = typeof(Stub).GetProperty("NonMember2");
            Assert.IsFalse(matching.IsMatch(property, 2));
        }

        [TestMethod]
        public void GetLookup_Positive()
        {
            var property = typeof(Stub).GetProperty("Member2");
            var matching = new DataMemberOrderMatchingStrategy();
            Assert.AreEqual(2, matching.GetLookup(property));
        }

        [TestMethod]
        public void GetLookup_Negative()
        {
            var property = typeof(Stub).GetProperty("NonMember");
            var matching = new DataMemberOrderMatchingStrategy();
            Assert.AreEqual(-1, matching.GetLookup(property));
            property = typeof(Stub).GetProperty("NonMember2");            
            Assert.AreEqual(-1, matching.GetLookup(property));
        }
    }
}
