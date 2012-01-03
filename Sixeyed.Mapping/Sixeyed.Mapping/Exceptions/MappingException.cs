using System;
using Sixeyed.Mapping.Extensions;

namespace Sixeyed.Mapping.Exceptions
{
    /// <summary>
    /// Represents an exception trying to map a property
    /// </summary>
    public class MappingException : Exception
    {
        /// <summary>
        /// Constructor with message format and args
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public MappingException(string messageFormat, params object[] args)
            : base(messageFormat.FormatWith(args))
        {
        }

        /// <summary>
        /// Constructor with exception, message format and args
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public MappingException(Exception innerException, string messageFormat, params object[] args)
            : base(messageFormat.FormatWith(args), innerException)
        {
        }
    }
}
