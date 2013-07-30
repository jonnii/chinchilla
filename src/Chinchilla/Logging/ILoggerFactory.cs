
namespace Chinchilla.Logging
{
    public interface ILoggerFactory
    {
        ILogger GetLogger<T>();
    }
}
