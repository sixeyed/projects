using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.Tests.Stubs;
using Sixeyed.Mapping.Tests.Stubs.Strategies;
using Sixeyed.Mapping.Tests.Stubs.Maps;
using System.Diagnostics;
using System.Reflection;
using System.Linq.Expressions;
using Sixeyed.Mapping.MatchingStrategies;
using Sixeyed.Mapping.CachingStrategies;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    [DeploymentItem("Stubs\\SourceData.xml")]
    public class XDocumentMapPerformanceTest : XDocumentMapTestBase
    {
        public int PerformanceIterations {get; set;}

        public XDocumentMapPerformanceTest()
        {
            PerformanceIterations = 250000;
        }

        [TestMethod]
        public void FromXml_Manual()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            var stream = new FileStream("SourceData.xml", FileMode.Open);
            var doc = XDocument.Load(stream);
            var root = doc.Root;
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var element = root.NextNode;
            }
            Debug.WriteLine("FromSqlCe_Manual -Mapped: {0} objects and asserted 0 failures in: {1}ms", count, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromXml_AutoMap()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            
            Debug.WriteLine("FromSqlCe_AutoMap -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromXml_AutoMap_Nested()
        { 
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            
            Debug.WriteLine("FromSqlCe_AutoMap_Nested -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        public void FromXml_StaticMap()
        {
            var stopwatch = Stopwatch.StartNew();
            var count = 0;
            
            Debug.WriteLine("FromSqlCe_StaticMap -Mapped: {0} objects and asserted 0 failures in: {1}ms", PerformanceIterations, stopwatch.ElapsedMilliseconds);
        }

        [TestMethod]
        //[Ignore] //use only to populate source for large result sets; 250K records = 80+Mb
        public void PopulateXml()
        {
            var writer = new XmlTextWriter("SourceData.xml", Encoding.Default);
            var root = new XElement("Users");
            for (int i = 0; i < PerformanceIterations; i++)
            {
                var user = GetFullUser();
                var userXml = GetUserXml(UserMixedXmlFormat, user);
                root.Add(XDocument.Parse(userXml).Root);
            }
            var source = new XDocument();
            source.Add(root);
            source.WriteTo(writer);
            writer.Flush();
            writer.Close();
        }
    }
}
