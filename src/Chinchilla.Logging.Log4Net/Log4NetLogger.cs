using System;
using log4net;
using log4net.Core;

namespace Chinchilla.Logging.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        /// <summary>log4net logger that this adapter wraps</summary>
        private readonly log4net.Core.ILogger logger;

        /// <summary>Used to exclude our logger from call site information</summary>
        private readonly static Type DeclaringType = typeof(Log4NetLogger);

        /// <summary>
        /// Wrap an instance of an log4net logger
        /// </summary>
        /// <param name="logger">log4net logger to wrap</param>
        public Log4NetLogger(ILog logger)
        {
            this.logger = logger.Logger;
        }

        /// <summary>
        /// Log to the wrapped log4net logger
        /// </summary>
        public void LogInternal(Level level, string message, Exception ex = null)
        {
            logger.Log(DeclaringType, level, message, ex);
        }

        public void Info(string message)
        {
            LogInternal(Level.Info, message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(Level.Info, message);
        }

        public void Debug(string message)
        {
            LogInternal(Level.Debug, message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(Level.Debug, message);
        }

        public void Error(Exception exception)
        {
            LogInternal(Level.Error, exception.Message, exception);
        }

        public void Error(Exception exception, string message)
        {
            LogInternal(Level.Error, message, exception);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            var message = string.Format(format, args);
            LogInternal(Level.Error, message, exception);
        }
    }
}
