using System;

namespace Chinchilla
{
    /// <summary>
    /// A delivery is a single message read from a queue. It must be
    /// accepted or rejected.
    /// </summary>
    public interface IDelivery
    {
        ulong Tag { get; }

        byte[] Body { get; }

        /// <summary>
        /// Indicates that this delivery has been processed and can be
        /// removed from the exchange
        /// </summary>
        void Accept();

        /// <summary>
        /// Called when a delivery fails
        /// </summary>
        /// <param name="e">The exception which caused this failure</param>
        void Failed(Exception e);
    }
}