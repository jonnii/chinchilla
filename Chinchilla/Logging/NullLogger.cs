namespace Chinchilla.Logging
{
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
    }
}