using System.Reflection;
using System.Runtime.Serialization;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MatchingStrategies
{
    /// <summary>
    /// Matching strategy which matches integers to the order specified on a property's <see cref="DataMemeberAttribute"/>
    /// </summary>
    public class DataMemberOrderMatchingStrategy : IMatchingStrategy<int>
    {
        /// <summary>
        /// Returns whether the order on the property's <see cref="DataMemberAttribute"/> matches the given integer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool IsMatch(PropertyInfo source, int target)
        {
            return (GetLookup(source) == target);
        }
        
        /// <summary>
        /// Returns the order specified on the property's <see cref="DataMemberAttribute"/>; -1 if none
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public int GetLookup(PropertyInfo target)
        {
            var lookup = -1;
            var dataMemberAttribute = target.GetFirstAttribute<DataMemberAttribute>();
            if (dataMemberAttribute != null)
            {
                lookup = dataMemberAttribute.Order;
            }
            return lookup;
        }
    }
}
