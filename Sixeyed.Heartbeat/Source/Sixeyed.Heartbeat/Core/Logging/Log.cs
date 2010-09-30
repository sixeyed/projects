using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Config;
using System.Threading;
using Sixeyed.Heartbeat.Extensions;

namespace Sixeyed.Heartbeat.Logging
{
    /// <summary>
    /// Logging class
    /// </summary>
    public static class Log
    {
        #region Private static fields
        private static ILog _logger;
        private static object syncLock = new object();
        #endregion

        #region Private static properties
        private static ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (syncLock)
                    {
                        //use double-checked locking:
                        if (_logger == null)
                        {
                            XmlConfigurator.Configure();
                            _logger = LogManager.GetLogger("Sixeyed.Heartbeat.Log");
                        }
                    }
                }
                return _logger;
            }
        }
        #endregion

        #region Public static methods

        /// <summary>
        /// Writes a debug-level message to the log, using the configured sink
        /// </summary>
        /// <remarks>
        /// The Func is only evaluated if the log level is active
        /// </remarks>
        /// <param name="message">Message to write</param>
        public static void Debug(Func<string> message)
        {
            WriteLog(LogLevel.Debug, message);
        }

        /// <summary>
        /// Writes a debug-level message to the log, using the configured sink
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="args">Message arguments</param>
        public static void Debug(string message, params object[] args)
        {
            WriteLog(LogLevel.Debug, () => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes an error-level message to the log, using the configured sink
        /// </summary>
        /// <remarks>
        /// The Func is only evaluated if the log level is active
        /// </remarks>
        /// <param name="message">Message to write</param>
        public static void Error(Func<string> message)
        {
            WriteLog(LogLevel.Error, message);
        }

        /// <summary>
        /// Writes a error-level message to the log, using the configured sink
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="args">Message arguments</param>
        public static void Error(string message, params object[] args)
        {
            WriteLog(LogLevel.Error, () => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes a fatal-level message to the log, using the configured sink
        /// </summary>
        /// <remarks>
        /// The Func is only evaluated if the log level is active
        /// </remarks>
        /// <param name="message">Message to write</param>
        public static void Fatal(Func<string> message)
        {
            WriteLog(LogLevel.Fatal, message);
        }

        /// <summary>
        /// Writes a fatal-level message to the log, using the configured sink
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="args">Message arguments</param>
        public static void Fatal(string message, params object[] args)
        {
            WriteLog(LogLevel.Fatal, () => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes an info-level message to the log, using the configured sink
        /// </summary>
        /// <remarks>
        /// The Func is only evaluated if the log level is active
        /// </remarks>
        /// <param name="message">Message to write</param>
        public static void Info(Func<string> message)
        {
            WriteLog(LogLevel.Info, message);
        }

        /// <summary>
        /// Writes an info-level message to the log, using the configured sink
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="args">Message arguments</param>
        public static void Info(string message, params object[] args)
        {
            WriteLog(LogLevel.Info, () => (message.FormatWith(args)));
        }

        /// <summary>
        /// Writes a warning-level message to the log, using the configured sink
        /// </summary>
        /// <remarks>
        /// The Func is only evaluated if the log level is active
        /// </remarks>
        /// <param name="message">Message to write</param>
        public static void Warn(Func<string> message)
        {
            WriteLog(LogLevel.Warn, message);
        }

        /// <summary>
        /// Writes a warning-level message to the log, using the configured sink
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="args">Message arguments</param>
        public static void Warn(string message, params object[] args)
        {
            WriteLog(LogLevel.Warn, () => (message.FormatWith(args)));
        }
        #endregion

        private static bool IsLogEnabled(LogLevel level)
        {
            var logEnabled = false;
            switch (level)
            {
                case LogLevel.Debug:
                    logEnabled = Logger.IsDebugEnabled;
                    break;
                case LogLevel.Error:
                    logEnabled = Logger.IsErrorEnabled;
                    break;
                case LogLevel.Fatal:
                    logEnabled = Logger.IsFatalEnabled;
                    break;
                case LogLevel.Info:
                    logEnabled = Logger.IsInfoEnabled;
                    break;
                case LogLevel.Warn:
                    logEnabled = Logger.IsWarnEnabled;
                    break;
            }
            return logEnabled;
        }

        private static void WriteLog(LogLevel level, Func<string> messageFunc)
        {
            if (IsLogEnabled(level))
            {
                var message = messageFunc();
                switch (level)
                {
                    case LogLevel.Debug:
                        Logger.Debug(message);
                        break;
                    case LogLevel.Error:
                        Logger.Error(message);
                        break;
                    case LogLevel.Fatal:
                        Logger.Fatal(message);
                        break;
                    case LogLevel.Info:
                        Logger.Info(message);
                        break;
                    case LogLevel.Warn:
                        Logger.Warn(message);
                        break;
                }
            }
        }

        private enum LogLevel
        {
            Debug,
            Error,
            Fatal,
            Info,
            Warn
        }
    }
}

