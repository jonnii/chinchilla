using System.Reflection;
using log4net;
using log4net.Config;

namespace Chinchilla.Logging.Log4Net
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        /// <summary>
        /// Initialises log4net from config file
        /// </summary>
        public Log4NetLoggerFactory()
        {
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()));
        }

        public ILogger GetLogger<T>()
        {
            return new Log4NetLogger(LogManager.GetLogger(typeof(T)));
        }
    }
}
