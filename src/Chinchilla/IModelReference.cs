using System;
using System.Collections.Concurrent;
using Chinchilla.Topologies.Rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public interface IModelReference : IDisposable
    {
        void Execute(Action<IModel> action);

        TR Execute<TR>(Func<IModel, TR> func);

        BlockingCollection<BasicDeliverEventArgs> StartConsuming(IQueue queue);
    }
}