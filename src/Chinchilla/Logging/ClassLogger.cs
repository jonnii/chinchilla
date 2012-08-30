using System;

namespace Chinchilla.Logging
{
    public class ClassLogger<T> : ILogger
    {
        public void Info(string message)
        {
            Logger.Target.Info(message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            Logger.Target.InfoFormat(format, args);
        }

        public void Debug(string message)
        {
            Logger.Target.Debug(message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            Logger.Target.DebugFormat(format, args);
        }

        public void Error(Exception exception)
        {
            Logger.Target.Error(exception);
        }

        public void Error(Exception exception, string message)
        {
            Logger.Target.Error(exception, message);
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            Logger.Target.ErrorFormat(exception, format, args);
        }
    }
}