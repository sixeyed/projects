using System.Collections.Generic;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.CachingStrategies
{
    /// <summary>
    /// Simple implementaion of <see cref="ICachingStrategy"/> - caching with an in-memory dictionary
    /// </summary>
    public class DictionaryCachingStrategy : ICachingStrategy
    {
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();

        /// <summary>
        /// Fetches an object from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object or null</returns>
        public object Get(string key)
        {
            object value = null;
            if (_cache.ContainsKey(key))
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
