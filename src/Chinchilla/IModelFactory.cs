using RabbitMQ.Client;

namespace Chinchilla
{
    public interface IModelFactory
    {
        IModelReference CreateModel();

        void Reconnect(IConnection newConnection);
    }
}