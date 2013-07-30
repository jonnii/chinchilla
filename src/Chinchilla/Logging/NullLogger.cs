using System;

namespace Chinchilla.Logging
{
    /// <summary>
    /// Default <see cref="ILogger"/> noop implementation
    /// </summary>
    public class NullLogger : ILogger
    {
        public void Info(string message)
        {
        }

        public void InfoFormat(string format, params object[] args)
        {
        }

        public void Debug(string message)
        {
        }

        public void DebugFormat(string format, params object[] args)
        {
        }

        public void Error(Exception exception)
        {
        }

        public void Error(Exception exception, string message)
        {
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
        }
    }
}