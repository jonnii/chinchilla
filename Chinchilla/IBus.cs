using System;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public interface IBus : IDisposable
    {
        ITopology Topology { get; }

        ISubscription Subscribe<T>(Action<T> onMessage);

        void Publish<T>(T message);
    }
}
