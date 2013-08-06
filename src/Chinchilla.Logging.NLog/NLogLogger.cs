using System;
using NLog;
using WrappedLogger = NLog.Logger;
using LogLevel = NLog.LogLevel;

namespace Chinchilla.Logging.NLog
{
    public class NLogLogger : ILogger
    {
        /// <summary>NLog logger that this adapter wraps</summary>
        private readonly WrappedLogger logger;

        /// <summary>Used to exclude our logger from call site information</summary>
        private readonly static Type DeclaringType = typeof(NLogLogger);

        /// <summary>
        /// Wrap an instance of an NLog logger
        /// </summary>
        /// <param name="logger">NLog logger to wrap</param>
        protected internal NLogLogger(WrappedLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Log to the wrapped NLog logger
        /// </summary>
        public void LogInternal(LogLevel level, string message, Exception ex = null)
        {
            var nlogEntry = new LogEventInfo
            {
                Level = level,
                TimeStamp = DateTime.UtcNow,
                Message = message,
                Exception = ex
            };

            logger.Log(DeclaringType, nlogEntry);
        }

        public void Info(string message)
        {
            LogInternal(LogLevel.Info, message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(LogLevel.Info, message);
        }

        public void Debug(string message)
        {
            LogInternal(LogLevel.Debug, message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(LogLevel.Debug, message);
        }

        public void Error(Exception exception)
        {
            LogInternal(LogLevel.Error, exception.Message, exception);
        }

        public void Error(Exception exception, string message)
        {
            LogInternal(LogLevel.Error, message, exception);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(LogLevel.Error, message, exception);
        }
    }
}
