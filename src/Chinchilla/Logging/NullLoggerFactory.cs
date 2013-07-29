
namespace Chinchilla.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger<T>()
        {
            return new NullLogger();
        }
    }
}