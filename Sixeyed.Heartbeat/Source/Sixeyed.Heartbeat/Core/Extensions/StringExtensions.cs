using System;
using System.Globalization;

namespace Sixeyed.Heartbeat.Extensions
{
    ///<summary>
    /// Extension methods for the <see cref="string"/> class.
    ///</summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the result of calling <seealso cref="string.Format(string,object[])"/> with the supplied arguments.
        /// </summary>
        /// <param name="formatString">The format string</param>
        /// <param name="args">The values to be formatted</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string formatString, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, formatString, args);
        }

        ///<summary>
        /// Tests the string to see if it is null or "".
        ///</summary>
        ///<param name="value">The string to test.</param>
        ///<returns>True if null or "".</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
