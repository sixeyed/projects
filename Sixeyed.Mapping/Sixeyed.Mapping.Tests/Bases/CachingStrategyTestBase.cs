using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.Mapping.Tests
{
    public abstract class CachingStrategyTestBase : TestBase
    {
        public class StubSource
        {            
            public string Prop1 { get; set; }

            [DataMember(Order=1)]
            public int Prop2 { get; set; }

            public bool Prop3 { get; set; }
        }

        public class StubTarget
        {
            public string Prop_1 { get; set; }

            public int Prop2 { get; set; }

            public bool Prop3 { get; set; }
        }

        protected static List<IPropertyMapping> GetMappings()
        {
            //add a random selection of mappings:
            var list = new List<IPropertyMapping>();
            var actionWithIrrelevantConversion = new ActionMapping<StubSource, StubTarget>((s, t) => s.Prop3 = t.Prop3);
            actionWithIrrelevantConversion.SetConversion<string, string>(x => x.ToUpper());
            list.Add(actionWithIrrelevantConversion);
            if (RandomBool())
            {
                list.Add(new FuncMapping<StubSource, string, StubTarget, string>(s => s.Prop1, t => t.Prop_1));
            }
            if (RandomBool())
            {
                Expression<Func<StubSource, int>> func = x => x.Prop2;
                list.Add(new CsvFieldMapping(1, func));
            }
            if (RandomBool())
            {
                list.Add(new DataReaderColumnMapping("PROP3", typeof(StubSource).GetProperty("Prop3")));
            }
            if (RandomBool())
            {
                list.Add(new ActionMapping<StubSource, StubTarget>((s, t) => s.Prop3 = t.Prop3));
            }
            if (RandomBool())
            {
                list.Add(new PropertyInfoMapping(typeof(StubSource).GetProperty("Prop3"), typeof(StubTarget).GetProperty("Prop3")));
            }
            if (RandomBool())
            {
                list.Add(new PropertyInfoMapping(typeof(StubSource).GetProperty("Prop3"), typeof(StubTarget).GetProperty("Prop3")));
            }
            if (RandomBool())
            {
                Expression<Func<StubSource, int>> func = x => x.Prop2;
                list.Add(new XAttributeMapping("Prop2", func));
            }
            if (RandomBool())
            {
                Expression<Func<StubSource, int>> func = x => x.Prop2;
                list.Add(new XElementMapping("Prop2", func));
            }
            if (RandomBool())
            {
                Expression<Func<StubSource, int>> func = x => x.Prop2;
                list.Add(new XPathMapping("/Source/Prop2", func));
            }
            return list;
        }

        protected static void AssertMappingsMatch(List<IPropertyMapping> original, List<IPropertyMapping> cached)
        {
            Assert.AreEqual(original.Count, cached.Count);
            for (int i = 0; i < original.Count; i++)
            {
                var o = original.ElementAt(i);
                var c = cached.ElementAt(i);
                Assert.AreEqual(o.GetType(), c.GetType());
                Assert.AreEqual(o.TargetName, c.TargetName);                
            }
        }
    }
}
