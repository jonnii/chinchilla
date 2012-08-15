using System;

namespace Chinchilla
{
    /// <summary>
    /// A publish channel is used to publish messages to an exchange
    /// </summary>
    public interface IPublishChannel : IPublisher, IDisposable
    {
        /// <summary>
        /// The number of published messages
        /// </summary>
        long NumPublishedMessages { get; }
    }
}