using System;

namespace Chinchilla.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void InfoFormat(string format, params object[] args);

        void Debug(string message);

        void DebugFormat(string format, params object[] args);

        void Error(Exception exception);

        void Error(Exception exception, string message);

        void ErrorFormat(Exception exception, string format, params object[] args);
    }
}
