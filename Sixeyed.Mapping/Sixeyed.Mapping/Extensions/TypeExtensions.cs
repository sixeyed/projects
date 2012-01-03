using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sixeyed.Mapping.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Type"/>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns a collection of <see cref="PropertyInfo"/> objects representing all 
        /// accessible public instance properties of the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAccessiblePublicInstanceProperties(this Type type)
        {
            var properties = from property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             where property.CanRead && property.GetGetMethod() != null
                             select property;
            return properties;
        }
    }
}
