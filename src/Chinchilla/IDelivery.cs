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
    }
}