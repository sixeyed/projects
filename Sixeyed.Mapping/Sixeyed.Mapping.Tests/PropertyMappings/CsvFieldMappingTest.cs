using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using Sixeyed.Mapping.CachingStrategies;
using Sixeyed.Mapping.MatchingStrategies;

namespace Sixeyed.Mapping.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CsvFieldMappingTest : TestBase
    {
        public string Property1 { get; set; }

        public int Property2 { get; set; }

        [TestMethod]
        public void ToString()
        {
            var index = Random.Next(1, 100);
            var target = this.GetType().GetProperty("Property1");
            var mapping = new CsvFieldMapping(index, target);
            Assert.AreEqual("CsvFieldMapping[Index: " + index + ", Target: System.String Property1, FieldDelimiter: ,]", mapping.ToString());
            
            index = Random.Next(1, 100);
            target = this.GetType().GetProperty("Property2");
            mapping = new CsvFieldMapping(index, target);
            mapping.FieldDelimiter = '|';
            Assert.AreEqual("CsvFieldMapping[Index: " + index + ", Target: Int32 Property2, FieldDelimiter: |]", mapping.ToString());
        
        }        
    }
}
