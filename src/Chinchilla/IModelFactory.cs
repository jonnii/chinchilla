using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public interface IModelFactory : IDisposable
    {
        IModelReference CreateModel();

        IModelReference CreateModel(string tag);

        void Reconnect(IConnection newConnection);
    }
}