using System;

namespace Chinchilla
{
    /// <summary>
    /// A fault strategy is invoked when a handler of consumer
    /// throws an exception while processing a message
    /// </summary>
    public interface IFaultStrategy
    {
        /// <summary>
        /// Handle a delivery failure
        /// </summary>
        /// <param name="delivery">The delivery that failed</param>
        /// <param name="exception">The exception that caused the delivery to fail</param>
        void ProcessFailedDelivery(IDelivery delivery, Exception exception);
    }
}
