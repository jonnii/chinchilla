using System;
using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public interface ISubscription : IDisposable
    {
        IQueue Queue { get; }
    }
}