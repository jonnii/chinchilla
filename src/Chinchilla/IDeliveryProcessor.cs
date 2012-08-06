namespace Chinchilla
{
    /// <summary>
    /// A delivery processor does the work of processing
    /// a single delivery.
    /// </summary>
    public interface IDeliveryProcessor
    {
        /// <summary>
        /// Processes a delivery
        /// </summary>
        void Process(IDelivery delivery);
    }
}