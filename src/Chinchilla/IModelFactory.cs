using RabbitMQ.Client;

namespace Chinchilla
{
    public interface IModelFactory
    {
        bool IsOpen { get; }

        IModel CreateModel();
    }
}