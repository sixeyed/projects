using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sixeyed.Mapping.Spec;
using Sixeyed.Mapping.MatchingStrategies;
using System.Reflection;

namespace Sixeyed.Mapping.Tests.Stubs.Strategies
{
    public class LegacyNameMatchingStrategy : AggressiveNameMatchingStrategy
    {
        public override string GetLookup(string input)
        {
            //assumes legacy db column names in the format:
            //<dataType>_<sensibleName>
            string[] parts = input.Split('_');
            if (parts.Length != 2)
                return base.GetLookup(parts[0]);
            else
                return base.GetLookup(parts[1]);
            
        }
    }
}
