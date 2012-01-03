using System.Reflection;

namespace Sixeyed.Mapping.Spec
{
    /// <summary>
    /// Represents a strategy for matching a target property from a source
    /// </summary>
    /// <typeparam name="TLookup">Type of object used to lookup the match</typeparam>
    public interface IMatchingStrategy<TLookup>
    {
        /// <summary>
        /// Gets the lookup value from the target property
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        TLookup GetLookup(PropertyInfo target);

        /// <summary>
        /// Whether the target property name is a match for the lookup value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        bool IsMatch(PropertyInfo target, TLookup source);
    }
}
