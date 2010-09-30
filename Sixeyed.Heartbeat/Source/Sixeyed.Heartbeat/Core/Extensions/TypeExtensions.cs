using System;
using Sixeyed.Heartbeat.Attributes;
using System.Reflection;

namespace Sixeyed.Heartbeat.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Type"/>
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns the first attribute on the field, matched on attribute type
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to return</typeparam>
        /// <param name="objectType">Type of object to reflect over</param>
        /// <param name="fieldName">Name of field to get attribute from</param>
        /// <returns>Attribute of specified type; first if there are multiple; null if none</returns>
        public static TAttribute GetFieldAttribute<TAttribute>(this Type type, string fieldName)
            where TAttribute : Attribute
        {
            FieldInfo field = type.GetField(fieldName, GetDefaultBindingFlags());
            return GetFirstAttribute<TAttribute>(field);
        }

        /// <summary>
        /// Returns the first attribute on the field, matched on attribute type
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to return</typeparam>
        /// <param name="info">MemberInfo to retrieve attributes from</param>
        /// <returns>Attribute of specified type; first if there are multiple; null if none</returns>
        private static TAttribute GetFirstAttribute<TAttribute>(MemberInfo info)
            where TAttribute : Attribute
        {
            TAttribute attribute = default(TAttribute);
            if (info != null)
            {
                object[] customAttributes = info.GetCustomAttributes(typeof(TAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    attribute = customAttributes[0] as TAttribute;
                }
            }
            return attribute;
        }

        private static BindingFlags GetDefaultBindingFlags()
        {
            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }
    }
}
