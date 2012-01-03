using System.Runtime.Caching;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.CachingStrategies
{
    /// <summary>
    /// Simple implementaion of <see cref="ICachingStrategy"/> - caching uses <see cref="MemoryCache"/>
    /// </summary>
    /// <remarks>
    /// Cache name used is Sixeyed.Mapping.MemoryCacheCachingStrategy
    /// </remarks>
    public class MemoryCacheCachingStrategy : ICachingStrategy
    {
        private static MemoryCache _cache = new MemoryCache("Sixeyed.Mapping.MemoryCacheCachingStrategy");

        /// <summary>
        /// Fetches an object from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object or null</returns>
        public object Get(string key)
        {
            object value = null;
            if (_cache.Contains(key))
            {
                value = _cache[key];
            }
            return value;
        }

        /// <summary>
        /// Adds or updates an object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Object to cache</param>
        public void Set(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
