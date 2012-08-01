namespace Chinchilla.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void InfoFormat(string format, params object[] args);

        void Debug(string message);

        void DebugFormat(string format, params object[] args);
    }
}
