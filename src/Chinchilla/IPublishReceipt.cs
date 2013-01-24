namespace Chinchilla
{
    /// <summary>
    /// A publish receipt is returned when publishing a message, it contains
    /// all the information regarding the state of the publish.
    /// </summary>
    public interface IPublishReceipt
    {
        /// <summary>
        /// The status of this publish receipt
        /// </summary>
        PublishStatus Status { get; }

        /// <summary>
        /// Indicates when this publish receipt has been confirmed
        /// </summary>
        bool IsConfirmed { get; }
    }
}