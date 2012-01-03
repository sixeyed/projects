using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.CachingStrategies
{
    /// <summary>
    /// Null implementation of <see cref="ICachingStrategy"/> - no cache used
    /// </summary>
    public class NullCachingStrategy : ICachingStrategy
    {
        /// <summary>
        /// Fetches an object from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>null</returns>
        public object Get(string key)
        {
            return null;
        }

        /// <summary>
        /// Adds or updates an object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Object to cache</param>
        public void Set(string key, object value)
        {
            //do nothing
        }
    }
}
