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
    public class CsvAutoMapTest : CsvMapTestBase
    {
        [TestMethod]
        public void AutoMapUnspecifiedTargets()
        {
            //automap should always automap:
            var map = new CsvAutoMap<User>();
            map.AutoMapUnspecifiedTargets = false;
            Assert.AreEqual(true, map.AutoMapUnspecifiedTargets);
        }

        [TestMethod]
        public void CsvToUser_AutoMap()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var mapped = new CsvAutoMap<User>().Create(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_AutoMap_WithMatching()
        {
            var original = GetFullUser();
            var csvRow = GetPropertyNameOrderedRow(original);
            var mapped = new CsvAutoMap<User>()
                             .Matching<PropertyDeclarationOrderMatchingStrategy>()
                             .Create(csvRow);
            AssertEqualByPropertyOrder(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_AutoMap_WithPipe()
        {
            var delimiter = '|';
            var original = GetFullUser();
            var csvRow = GetCsvRow(original, delimiter);
            var map = new CsvAutoMap<User>().WithFieldDelimiter(delimiter);
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_AutoMap_WithCache()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var map = new CsvAutoMap<User>().Cache <DictionaryCachingStrategy>();
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_WithSpecify()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var map = new CsvAutoMap<User>()
                            .Specify(5, t => t.Address.Line1)
                            .Specify<string, string>(6, t => t.Address.Line2, x => x.ToUpper());
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
            Assert.AreEqual(original.Address.Line1, mapped.Address.Line1);
            Assert.AreEqual(original.Address.Line2.ToUpper(), mapped.Address.Line2);
        }

        [TestMethod]
        public void CsvToUser_WithSpecifyAction()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var map = new CsvAutoMap<User>()
                            .Specify(5, t => t.Address.Line1)
                            .Specify((s, t) => t.Address.Line2 = csvRow.Split(',')[5].ToUpper());
            var mapped = map.Create(csvRow);
            AssertEqual(original, mapped);
            Assert.AreEqual(original.Address.Line1, mapped.Address.Line1);
            Assert.AreEqual(original.Address.Line2.ToUpper(), mapped.Address.Line2);
        }

        [TestMethod]
        public void CsvToUser_StaticAutoMap()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var mapped = CsvAutoMap<User>.CreateTarget(csvRow);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUser_StaticAutoMap_Populate()
        {
            var original = GetFullUser();
            var csvRow = GetCsvRow(original);
            var mapped = new User();
            CsvAutoMap<User>.PopulateTarget(csvRow, mapped);
            AssertEqual(original, mapped);
        }

        [TestMethod]
        public void CsvToUserList_StaticAutoMap()
        {
            var count = Random.Next(10, 20);
            var originals = new List<User>();
            var csvLines = new List<string>();            
            for (int i = 0; i < count; i++)
            {
                var user = GetFullUser();
                originals.Add(user);
                csvLines.Add(GetCsvRow(user));                
            }
            var mapped = CsvAutoMap<User>.CreateTargetList(csvLines);
            for (int i = 0; i < count; i++)
            {
                AssertEqual(originals[i], mapped[i]);
            }            
        }        
    }
}
