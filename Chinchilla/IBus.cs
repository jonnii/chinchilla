using System;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public interface IBus : IPublisher, IDisposable
    {
        ITopology Topology { get; }

        IPublishChannel CreatePublishChannel();

        ISubscription Subscribe<T>(Action<T> onMessage);
    }
}
