using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public interface IModelFactory : IDisposable
    {
        IModelReference CreateModel();

        void Reconnect(IConnection newConnection);
    }
}