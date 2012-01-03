using System;
using System.Reflection;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MappingStrategies
{
    /// <summary>
    /// Mapping stratgey for mapping from a <see cref="string"/> input containing delimited fields
    /// </summary>
    /// <remarks>
    /// Source is a string, index for looking up field mappings is integer - 
    /// representing the index of the field in the CSV string
    /// </remarks>
    public class CsvMappingStrategy : MappingStrategy<string, int, int>
    {
        /// <summary>
        /// Gets/sets the delimiter for the CSV input
        /// </summary>
        public char FieldDelimiter { get; set; }

        protected override void CreateMappings(string source, Type targetType, IMatchingStrategy<int> matching)
        {
            int fieldCount = source.Split(FieldDelimiter).Length;

            var targetProperties = targetType.GetAccessiblePublicInstanceProperties();
            foreach (var target in targetProperties)
            {
                for (int i = 1; i < (fieldCount+1); i++)
                {
                    if (matching.IsMatch(target, i))
                    {
                        AddMapping(CreateMapping(i, target));
                        break;
                    }
                }
            }
        }

        protected override IPropertyMapping CreateMapping(int sourceIndex, PropertyInfo target)
        {
            var mapping = new CsvFieldMapping(sourceIndex, target);
            mapping.FieldDelimiter = FieldDelimiter;
            return mapping;
        }

        protected override void OnBeforePopulate(IPropertyMapping mapping, object target, object source)
        {
            if (mapping is CsvFieldMapping)
            {
                ((CsvFieldMapping)mapping).FieldDelimiter = FieldDelimiter;
            }
        }
    }
}
