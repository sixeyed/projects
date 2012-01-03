using System;
using System.Linq.Expressions;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping
{
    /// <summary>
    /// Property mapping using an indexed field within a delimited string as source
    /// </summary>
    [Serializable]
    public class CsvFieldMapping : PropertyMapping<int>
    {
        /// <summary>
        /// Gets/sets field delimiter
        /// </summary>
        /// <remarks>
        /// Defaults to comma 
        /// </remarks>
        public char FieldDelimiter { get; set; }

        /// <summary>
        /// Constructor with source index and target property
        /// </summary>
        /// <param name="fieldIndex">Source field index</param>
        /// <param name="target">Target property</param>
        public CsvFieldMapping(int fieldIndex, PropertyInfo target) : base(fieldIndex, target) 
        {
            FieldDelimiter = ',';
        }

        /// <summary>
        /// Constructor with source index and target accessor
        /// </summary>
        /// <param name="fieldIndex">Source field index</param>
        /// <param name="targetExpression">Target accessor</param>
        public CsvFieldMapping(int fieldIndex, LambdaExpression targetExpression) : base(fieldIndex, targetExpression) 
        {
            FieldDelimiter = ',';
        }

        /// <summary>
        /// Whether the source field can be read
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override bool CanRead(object source)
        {
            return (source is string);
        }        

        protected override object GetValueInternal(object source)
        {
            object value = null;
            if (CanRead(source))
            {
                var sourceFields = GetSourceFields((string)source);
                value = sourceFields[Source-1];
            }
            return value;
        }

        private string[] GetSourceFields(string csvRow)
        {
            return csvRow.Split(FieldDelimiter);
        }

        public override string ToString()
        {
            return "CsvFieldMapping[Index: {0}, Target: {1}, FieldDelimiter: {2}]".FormatWith(Source, TargetPropertyInfo, FieldDelimiter);
        }
    }
}