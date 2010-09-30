using System;

namespace Sixeyed.Heartbeat.TestFramework
{
    public static class RandomValueGenerator
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns></returns>
        public static bool GetRandomBool()
        {
            return random.Next() % 2 == 0;
        }

        /// <summary>
        /// Returns a random non-negative integer value
        /// </summary>
        /// <returns></returns>
        public static int GetRandomInt()
        {
            return random.Next();
        }

        /// <summary>
        /// Returns a random non-negative integer value
        /// </summary>
        /// <param name="maxValue">Maximum value to return</param>
        /// <returns></returns>
        public static int GetRandomInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        /// <summary>
        /// Returns a random non-negative integer value
        /// </summary>
        /// <param name="minValue">Minimum value to return</param>
        /// <param name="maxValue">Maximum value to return</param>
        /// <returns></returns>
        public static int GetRandomInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Gets a random date between 01.01.1900 and today
        /// </summary>
        /// <returns></returns>
        public static DateTime GetRandomDateTime()
        {
            var year = GetRandomInt(1900, DateTime.Now.Year);
            return GetRandomDateTime(year);
        }

        /// <summary>
        /// Gets a random date in the gven year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime GetRandomDateTime(int year)
        {
            var month = GetRandomInt(1, 12);
            var day = GetRandomInt(DateTime.DaysInMonth(year, month));
            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Gets a random string of alphanumeric characters containing spaces
        /// </summary>
        /// <returns></returns>
        public static string GetRandomString()
        {
            return GetRandomString(false);
        }

        /// <summary>
        /// Gets a random string of alphanumeric characters
        /// </summary>
        /// <param name="excludeSpaces">Whether to exclude spaces from the string</param>
        /// <returns></returns>
        public static string GetRandomString(bool excludeSpaces)
        {
            return Guid.NewGuid().ToString().Replace("-", excludeSpaces ? string.Empty : " ");
        }

        /// <summary>
        /// Gets a random string of alphanumeric characters 
        /// </summary>
        /// <param name="maxLength">Maximum string length</param>
        /// <param name="excludeCharacters">Characters to exclude from the string</param>
        /// <returns></returns>
        public static string GetRandomString(int maxLength, params string[] excludeCharacters)
        {
            var length = GetRandomInt(maxLength);
            var randomString = GetRandomString(excludeCharacters);
            return randomString.Substring(0, length);
        }

        /// <summary>
        /// Gets a random string of alphanumeric characters 
        /// </summary>
        /// <param name="excludeCharacters">Characters to exclude from the string</param>
        /// <returns></returns>
        public static string GetRandomString(params string[] excludeCharacters)
        {
            var randomString = GetRandomString();
            foreach (var character in excludeCharacters)
            {
                randomString = randomString.Replace(character, string.Empty);
            }
            return randomString;
        }

        /// <summary>
        /// Gets a random enum value
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <returns></returns>
        public static TEnum GetRandomEnumValue<TEnum>()
        {
            var values = Enum.GetValues(typeof(TEnum));
            var randomIndex = GetRandomInt(values.Length);            
            return (TEnum) values.GetValue(randomIndex);
        }
    }
}
