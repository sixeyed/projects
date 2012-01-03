
namespace Sixeyed.Mapping.MatchingStrategies
{
    /// <summary>
    /// Matching strategy which matches exact strings
    /// </summary>
    public class ExactNameMatchingStrategy : NameMatchingStrategy
    {
        /// <summary>
        /// Returns the input string, unmodified
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override string GetLookup(string input)
        {
            return input;
        }
    }
}
