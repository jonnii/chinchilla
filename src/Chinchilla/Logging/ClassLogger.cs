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
    }
}