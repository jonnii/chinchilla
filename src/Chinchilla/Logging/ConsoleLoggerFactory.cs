
namespace Chinchilla.Logging
{
    public class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger<T>()
        {
            return new ConsoleLogger();
        }
    }
}