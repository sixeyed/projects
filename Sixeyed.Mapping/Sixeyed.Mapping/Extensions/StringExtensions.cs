using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sixeyed.Mapping.Extensions
{
    ///<summary>
    /// Extension methods for the <see cref="string"/> class.
    ///</summary>
    public static class StringExtensions
    {
        private static Regex _PropertyNameRegex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
        private static List<char> _UpperVowels = new List<char>(new char[] { 'A', 'E', 'I', 'O', 'U' });
        private static List<char> _LowerVowels = new List<char>(new char[]{ 'a', 'e', 'i', 'o', 'u' });

        /// <summary>
        /// Returns the result of calling <seealso cref="string.Format(string,object[])"/> with the supplied arguments.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="CultureInfo.InvariantCulture"/> to format
        /// </remarks>
        /// <param name="formatString">The format string</param>
        /// <param name="args">The values to be formatted</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string formatString, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, formatString, args);
        }

        /// <summary>
        /// Retuirns the input string with vowels stripped
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveVowels(this string input)
        {
            var builder = new StringBuilder(input.Length);
            foreach (var character in input)
            {
                if (!_UpperVowels.Contains(character) && !_LowerVowels.Contains(character))
                {
                    builder.Append(character);
                }
            }
            return builder.ToString();
        }

        public static string RemoveDuplicateCharacters(this string input)
        {
            var builder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                if (i < input.Length - 1 && input[i] != input[i + 1])
                {
                    builder.Append(input[i]);
                }
                else if (i > 0 && i == input.Length - 1)
                {
                    builder.Append(input[i]);
                }
            }
            return builder.ToString();
        }

        public static string RemoveIllegalPropertyNameCharacters(this string input)
        {
            return _PropertyNameRegex.Replace(input, string.Empty);
        }
    }
}
