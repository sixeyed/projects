using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using a named column in a data reader as source
    /// </summary>
    [Serializable]
    public class DataReaderColumnMapping : PropertyMapping<string>
    {
        /// <summary>
        /// Constructor with column name and target property
        /// </summary>
        /// <param name="sourceColumnName">Source column name</param>
        /// <param name="target">Target property</param>
        public DataReaderColumnMapping(string sourceColumnName, PropertyInfo target) : base(sourceColumnName, target) { }

        /// <summary>
        /// Constructor with source column name and target accessor
        /// </summary>
        /// <param name="sourceColumnName">Source column name</param>
        /// <param name="targetExpression">Target accessor</param>
        public DataReaderColumnMapping(string sourceColumnName, LambdaExpression targetExpression) : base(sourceColumnName, targetExpression) { }

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanRead(object source)
        {
            return (source is IDataReader);
        }

        protected override object GetValueInternal(object source)
        {
            object value = null;
            if (CanRead(source))
            {
                var sourceReader = source as IDataReader;
                var columnIndex = sourceReader.GetOrdinal(Source);
                value = sourceReader.GetValue(columnIndex);
            }
            return value;
        }
    }
}