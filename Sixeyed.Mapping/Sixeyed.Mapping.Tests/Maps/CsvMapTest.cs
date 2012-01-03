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
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CsvMapTest : CsvMapTestBase
    {    
        [TestMethod]
        public void CsvToUser()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var map = new FullUserFromCsvMap();
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);            
        }

        [TestMethod]
        public void CsvToUser_WithMatching()
        {
            var original = GetFullUser();
            var csvRow = GetPropertyNameOrderedRowWithAddress(original);
            var map = new FullUserFromOrderedCsvMap().Matching<PropertyDeclarationOrderMatchingStrategy>();

            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_WithDelimiter()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original, '|');
            var map = new FullUserFromCsvMap();
            map.WithFieldDelimiter('|');
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_WithCache()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var map = new FullUserFromCsvMap().Cache<DictionaryCachingStrategy>();
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
        }
    }
}
