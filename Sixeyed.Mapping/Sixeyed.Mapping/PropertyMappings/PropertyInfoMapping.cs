using System;
using System.Reflection;
using Sixeyed.Mapping.PropertyMappings.DelegateWrappers;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping with specified properties for source and target values
    /// </summary>
    [Serializable]
    public class PropertyInfoMapping : PropertyMapping<PropertyInfo>
    {
        /// <summary>
        /// Constructor with source propety and target property
        /// </summary>
        /// <param name="source">Source property</param>
        /// <param name="target">Target property</param>
        public PropertyInfoMapping(PropertyInfo source, PropertyInfo target) : base(source, target) { }

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanRead(object source)
        {
            return (source != null);
        }

        protected override object GetValueInternal(object source)
        {
            object value = null;
            if (CanRead(source))
            {
                value = GetDelegateWrapper.GetValue(source, Source);
            }
            return value;
        }
    }
}