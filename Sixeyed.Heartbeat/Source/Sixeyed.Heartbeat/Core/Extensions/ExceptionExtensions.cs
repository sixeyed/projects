using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Sixeyed.Heartbeat.Extensions
{
    /// <summary>
    /// Extensions to <see cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns the full message for the exception stack
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns></returns>
        public static string FullMessage(this Exception exception)
        {
            return exception.FullMessage(Environment.NewLine);
        }

        /// <summary>
        /// Returns the full message for the exception stack
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="separator">Separator to use between inner exceptions</param>
        /// <returns></returns>
        public static string FullMessage(this Exception exception, string separator)
        {
            var builder = new StringBuilder();
            while (exception != null)
            {
                builder.AppendFormat("{0}{1}", exception.Message, separator);
                exception = exception.InnerException;
            }
            return builder.ToString();
        }
    }
}
