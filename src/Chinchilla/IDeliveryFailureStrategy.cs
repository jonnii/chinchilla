using System;

namespace Chinchilla
{
    /// <summary>
    /// A delivery failure strategy is invoked when a handler of consumer
    /// throws an exception while processing a message
    /// </summary>
    public interface IDeliveryFailureStrategy
    {
        void Handle(IDelivery delivery, Exception exception);
    }
}
