using System;

namespace Chinchilla
{
    /// <summary>
    /// A delivery is a single message read from a queue. It must be
    /// accepted or rejected.
    /// </summary>
    public interface IDelivery
    {
        /// <summary>
        /// The delivery tag
        /// </summary>
        ulong Tag { get; }

        /// <summary>
        /// The raw body of the delivery
        /// </summary>
        byte[] Body { get; }

        /// <summary>
        /// The original routing key for this delivery
        /// </summary>
        string RoutingKey { get; }

        /// <summary>
        /// The exchange that this message was routed through
        /// </summary>
        string Exchange { get; }

        /// <summary>
        /// The content type of this delivery
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Indicates that this delivery is replyable
        /// </summary>
        bool IsReplyable { get; }

        /// <summary>
        /// The correlation id for this delivery
        /// </summary>
        string CorrelationId { get; }

        /// <summary>
        /// The reply to address on this delivery
        /// </summary>
        string ReplyTo { get; }

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