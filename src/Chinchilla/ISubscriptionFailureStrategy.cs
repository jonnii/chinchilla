using System;

namespace Chinchilla
{
    /// <summary>
    /// A failure strategy is invoked when a handler or consumer
    /// throws an exception while processing a message
    /// </summary>
    public interface ISubscriptionFailureStrategy
    {
        /// <summary>
        /// Handle a delivery failure
        /// </summary>
        /// <param name="delivery">The delivery that failed</param>
        /// <param name="exception">The exception that caused the delivery to fail</param>
        void OnFailure(IDelivery delivery, Exception exception);
    }
}
