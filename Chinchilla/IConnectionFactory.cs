using System;
using RabbitMQ.Client;

namespace Chinchilla
{
    public interface IConnectionFactory
    {
        IConnection Create(Uri uri);
    }
}