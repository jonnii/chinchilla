using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface IPublisherBuilder
    {
        IPublisherBuilder SetTopology(Func<Endpoint, IPublisherTopology> customTopology);
    }
}