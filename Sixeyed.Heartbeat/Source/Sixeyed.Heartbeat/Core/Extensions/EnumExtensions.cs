using System;
using Sixeyed.Heartbeat.Attributes;

namespace Sixeyed.Heartbeat.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Enum"/>
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the value of the <see cref="DatabaseValueAttribute"/> flagged on the enum value
        /// </summary>
        /// <remarks>
        /// Returns <see cref="String.Empty"/> if the enum value does not have a <see cref="DatabaseValueAttribute"/>
        /// </remarks>
        /// <param name="enumValue"></param>
        /// <returns>Database value</returns>
        public static string ToDatabaseValue(this Enum enumValue)            
        {
            return GetAttributeValue<DatabaseValueAttribute>(enumValue);
        }

        /// <summary>
        /// Returns the value of the given attribute flagged on the enum value
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to retrieve</typeparam>
        /// <param name="enumValue">Enum value</param>
        /// <returns>Value of the attribute</returns>
        public static string GetAttributeValue<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            var enumType = enumValue.GetType();
            var attribute = enumType.GetFieldAttribute<TAttribute>(Enum.GetName(enumType, enumValue));
            if (attribute == null)
                return string.Empty;
            return attribute.ToString();
        }

        /// <summary>
        /// Returns the enum value which is flagged with a <see cref="DatabaseValueAttribute"/> with the given value
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="databaseValue"></param>
        /// <returns>Enum value</returns>
        public static TEnum FromDatabaseValue<TEnum>(string databaseValue)
        {
            TEnum enumValue = default(TEnum);
            var enumType = typeof(TEnum);
            foreach (string value in Enum.GetNames(enumType))
            {
                var dbValue = enumType.GetFieldAttribute<DatabaseValueAttribute>(value);
                if (dbValue != null && !dbValue.DatabaseValue.IsNullOrEmpty() && dbValue.DatabaseValue == databaseValue)
                {
                    enumValue = (TEnum) Enum.Parse(enumType, value);
                }
            }
            return enumValue;
        }
    }
}
