using System.Reflection;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MatchingStrategies
{
    /// <summary>
    /// Base class for matchign strings
    /// </summary>
    public abstract class NameMatchingStrategy : IMatchingStrategy<string>
    {
        /// <summary>
        /// Returns a lookup version of the given string for matching
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract string GetLookup(string input);

        /// <summary>
        /// Returns a lookup version of the property name for matching
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetLookup(PropertyInfo target)
        {
            return GetLookup(target.Name);
        }

        /// <summary>
        /// Returns whether the name of the property and the given string are a match
        /// </summary>
        /// <param name="target"></param>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        public virtual bool IsMatch(PropertyInfo target, string sourceName)
        {
            return (target.Name == sourceName) || (GetLookup(target) == (GetLookup(sourceName)));
        }
    }
}
