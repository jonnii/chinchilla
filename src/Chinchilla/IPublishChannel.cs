using System;

namespace Chinchilla
{
    public interface IPublishChannel : IPublisher, IDisposable
    {
        long NumPublishedMessages { get; }
    }
}