using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sixeyed.Mapping.CachingStrategies;

namespace Sixeyed.Mapping.Tests
{
    [TestClass]
    public class MemoryCacheCachingStrategyTest : CachingStrategyTestBase
    {
        [TestMethod]
        public void SetThenGet()
        {
            var cacheKey = "DictionaryCachingStrategyTest.SetThenGet";
            var mappings = GetMappings();
            var cache = new MemoryCacheCachingStrategy();
            cache.Set(cacheKey, mappings);
            var retrievedMappings = (List<IPropertyMapping>)cache.Get(cacheKey);
            AssertMappingsMatch(mappings, retrievedMappings);
        }
    }
}
