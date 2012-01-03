using System.Reflection;
using System;

namespace Sixeyed.Mapping.Extensions
{
    /// <summary>
    /// Extensions to <see cref="PropertyInfo"/>
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Returns the first custom attribute of the given type applied to the property
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns>First instance of TAttribute, or null</returns>
        public static TAttribute GetFirstAttribute<TAttribute>(this PropertyInfo propertyInfo)
            where TAttribute : Attribute
        {
            TAttribute attribute = default(TAttribute);
            if (propertyInfo != null)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), false);
                if (customAttributes.Length > 0)
                {
                    attribute = customAttributes[0] as TAttribute;
                }
            }
            return attribute;
        }
    }
}
