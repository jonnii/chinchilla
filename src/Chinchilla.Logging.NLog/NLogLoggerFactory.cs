using NLog;

namespace Chinchilla.Logging.NLog
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger<T>()
        {
            return new NLogLogger(LogManager.GetLogger(typeof(T).FullName));
        }
    }
}
