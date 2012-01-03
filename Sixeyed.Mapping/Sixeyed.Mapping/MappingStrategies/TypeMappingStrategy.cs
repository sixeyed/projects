using System;
using System.Collections.Generic;
using System.Reflection;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MappingStrategies
{
    /// <summary>
    /// Mapping stratgey for mapping from a <see cref="Type"/> input 
    /// </summary>
    /// <remarks>
    /// Source is a type, index for looking up field mappings is a string - 
    /// representing the name of a field in the type's property info 
    /// </remarks>
    public class TypeMappingStrategy : MappingStrategy<Type, PropertyInfo, string>
    {
        protected override void CreateMappings(Type sourceType, Type targetType, IMatchingStrategy<string> matching)
        {
            var sourceProperties = sourceType.GetAccessiblePublicInstanceProperties();
            var targetProperties = targetType.GetAccessiblePublicInstanceProperties();
            var sourcePropertyLookup = new Dictionary<string, PropertyInfo>();
            foreach (var source in sourceProperties)
            {
                sourcePropertyLookup.Add(matching.GetLookup(source), source);
            }
            foreach (var target in targetProperties)
            {
                CreateMapping(matching, sourcePropertyLookup, target);
            }
        }

        private void CreateMapping(IMatchingStrategy<string> matching, Dictionary<string, PropertyInfo> sourcePropertyLookup, PropertyInfo target)
        {
            var targetName = matching.GetLookup(target);
            if (sourcePropertyLookup.ContainsKey(targetName) &&
                sourcePropertyLookup[targetName].PropertyType == target.PropertyType)
            {
                AddMapping(CreateMapping(sourcePropertyLookup[targetName], target));
            }
        }

        protected override IPropertyMapping CreateMapping(PropertyInfo sourceItem, PropertyInfo target)
        {
            return new PropertyInfoMapping(sourceItem, target);
        }

        protected override Type GetSource(object sourceType)
        {
            return sourceType.GetType();
        }
    }
}
