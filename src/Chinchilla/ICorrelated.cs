using System;

namespace Chinchilla
{
    /// <summary>
    /// Indicates that this message has correlated. Correlated messages
    /// are used to identify related messages
    /// </summary>
    public interface ICorrelated
    {
        /// <summary>
        /// The correlation id for this message
        /// </summary>
        Guid CorrelationId { get; }
    }
}