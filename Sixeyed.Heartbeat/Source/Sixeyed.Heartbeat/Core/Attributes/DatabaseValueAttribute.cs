using System;

namespace Sixeyed.Heartbeat.Attributes
{
    /// <summary>
    /// Attribute for identifying the database value for a field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=false, Inherited=true)]
    public sealed class DatabaseValueAttribute : Attribute
    {
        /// <summary>
        /// Database value
        /// </summary>
        public string DatabaseValue { get; private set; }

        /// <summary>
        /// Constructor with known state
        /// </summary>
        /// <param name="databaseValue">Database value</param>
        public DatabaseValueAttribute(string databaseValue)
        {
            DatabaseValue = databaseValue;
        }

        /// <summary>
        /// Returns the database value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DatabaseValue;
        }
    }
}
