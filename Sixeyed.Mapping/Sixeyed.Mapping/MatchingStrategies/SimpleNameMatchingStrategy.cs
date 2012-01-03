using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.MatchingStrategies
{
    /// <summary>
    /// Matching strategy which matches strings, ignoring non-alphanumeric characters and case
    /// </summary>
    /// <remarks>
    /// Matches "Property_Number_1" and "PROPERTYNUMBER1"
    /// </remarks>
    public class SimpleNameMatchingStrategy : NameMatchingStrategy
    {
        /// <summary>
        /// Returns the input string uppercased and with non-alphanumeric characters stripped
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string GetLookup(string input)
        {
            return input.Trim()
                        .RemoveIllegalPropertyNameCharacters()
                        .Replace("_", string.Empty)
                        .ToUpperInvariant();
        }
    }
}
