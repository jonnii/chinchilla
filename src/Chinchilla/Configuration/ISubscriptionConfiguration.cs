using System.Collections.Generic;

namespace Chinchilla.Configuration
{
    public interface ISubscriptionConfiguration : IEndpointConfiguration
    {
        string Name { get; }

        uint PrefetchSize { get; }

        ushort PrefetchCount { get; }

        IEnumerable<string> EndpointNames { get; }

        IDeliveryStrategy BuildDeliveryStrategy(IDeliveryProcessor deliveryProcessor);

        IFaultStrategy BuildFaultStrategy(IBus bus);
    }
}