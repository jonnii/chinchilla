using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public interface IPublisherBuilder
    {
        IPublisherBuilder SetTopology(Func<string, IPublisherTopology> customTopology);
    }
}