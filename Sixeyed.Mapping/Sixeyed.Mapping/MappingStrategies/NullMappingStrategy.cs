using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.Mapping.Spec;
using System.Reflection;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Strategies
{
    public class NullMappingStrategy : IMappingStrategy<string>
    {
        public bool GeneratedMappings { get { return true; } }

        public bool RunInParallel { get; set; }

        public List<IPropertyMapping> GetMappings(object source, Type targetType, IMatchingStrategy<string> matching, bool autoMapMissingTargets)
        {
            return new List<IPropertyMapping>();
        }

        public void SetMappings(List<IPropertyMapping> mappings)
        {
            //do nothing
        }

        public void AddMapping(IPropertyMapping mapping)
        {
            //do nothing
        }

        public void AddMapping(PropertyInfo target, object sourceItem)
        {
            //do nothing
        }

        public void Populate(object target, object source)
        {
            //do nothing
        }
    }
}
