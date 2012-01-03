using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    public class DictionaryCachingStrategyTest : CachingStrategyTestBase
    {
        [TestMethod]
        public void SetThenGet()
        {
            var cacheKey = "DictionaryCachingStrategyTest.SetThenGet";
            var mappings = GetMappings();
            var cache = new DictionaryCachingStrategy();
            cache.Set(cacheKey, mappings);
            var retrievedMappings = (List<IPropertyMapping>)cache.Get(cacheKey);
            AssertMappingsMatch(mappings, retrievedMappings);
        }
    }
}
