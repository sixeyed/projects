using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Sixeyed.Mapping.Spec;
using Sixeyed.Mapping.Extensions;
using System.Runtime.Serialization;

namespace Sixeyed.Mapping.Tests.Stubs.Strategies
{
    public class PropertyDeclarationOrderMatchingStrategy : IMatchingStrategy<int>
    {
        public bool IsMatch(PropertyInfo source, int target)
        {
            return (GetLookup(source) == target);
        }
        
        public int GetLookup(PropertyInfo target)
        {
            var lookup = -1;            
            var properties = target.DeclaringType.GetProperties();
            for (int i = 0; i < properties.Count(); i++)
            {
                if (properties.ElementAt(i).Name == target.Name)
                {
                    lookup = i + 1;
                    break;
                }
            }            
            return lookup;
        }
    }
}
