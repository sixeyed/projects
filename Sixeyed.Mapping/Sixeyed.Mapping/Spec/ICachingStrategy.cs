
namespace Sixeyed.Mapping.Spec
{
    /// <summary>
    /// A strategy for generic access to a cache
    /// </summary>
    public interface ICachingStrategy
    {
        /// <summary>
        /// Fetches an object from the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object or null</returns>
        object Get(string key);

        /// <summary>
        /// Adds or updates an object to the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Object to cache</param>
        void Set(string key, object value);
    }
}
