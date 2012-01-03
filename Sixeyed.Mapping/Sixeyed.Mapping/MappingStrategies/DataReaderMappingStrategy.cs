using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Sixeyed.Mapping.Extensions;
using Sixeyed.Mapping.Spec;

namespace Sixeyed.Mapping.MappingStrategies
{
    /// <summary>
    /// Mapping stratgey for mapping from an <see cref="IDataReader"/> input
    /// </summary>
    /// <remarks>
    /// Source is a data reader, index for looking up field mappings is string - 
    /// representing the name of the column
    /// </remarks>
    public class DataReaderMappingStrategy : MappingStrategy<IDataReader, string, string>
    {
        protected override void CreateMappings(IDataReader source, Type targetType, IMatchingStrategy<string> matching)
        {
            List<string> columnNames = new List<string>(source.FieldCount);
            for (int i = 0; i < source.FieldCount; i++)
            {
                columnNames.Add(source.GetName(i));
            }

            var targetProperties = targetType.GetAccessiblePublicInstanceProperties();
            foreach (var target in targetProperties)
            {
                foreach (string columnName in columnNames)
                {
                    if (matching.IsMatch(target, columnName))
                    {
                        AddMapping(CreateMapping(columnName, target));
                        break;
                    }
                }
            }
        }

        protected override IPropertyMapping CreateMapping(string sourceItem, PropertyInfo target)
        {
            return new DataReaderColumnMapping(sourceItem, target);
        }
    }
}
