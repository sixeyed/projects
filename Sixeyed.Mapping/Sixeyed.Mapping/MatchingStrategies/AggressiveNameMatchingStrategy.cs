using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.MatchingStrategies
{
    /// <summary>
    /// Matching strategy which matches strings, ignoring non-alphanumeric characters, case, vowels and duplicate characters
    /// </summary>
    /// <remarks>
    /// Matches "Property_Number_1" and "PROPARTYNOMBAR1"
    /// </remarks>
    public class AggressiveNameMatchingStrategy: NameMatchingStrategy
    {
        /// <summary>
        /// Returns the input string uppercased and with non-alphanumeric characters, vowels and duplicates stripped
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string GetLookup(string input)
        {
            return input.Trim()
                        .RemoveIllegalPropertyNameCharacters()
                        .RemoveDuplicateCharacters()
                        .RemoveVowels()
                        .Replace("_", string.Empty)
                        .ToUpperInvariant();
        }
    }
}
