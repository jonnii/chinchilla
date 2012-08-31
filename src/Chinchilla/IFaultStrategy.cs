using System;

namespace Chinchilla
{
    /// <summary>
    /// A fault strategy is invoked when a handler of consumer
    /// throws an exception while processing a message
    /// </summary>
    public interface IFaultStrategy
    {
        void Handle(IDelivery delivery, Exception exception);
    }
}
