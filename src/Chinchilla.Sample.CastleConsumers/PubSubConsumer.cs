using System;
using Chinchilla.Configuration;

namespace Chinchilla.Sample.CastleConsumers
{
    public class PubSubConsumer : IConsumer<PubSubMessage>, IConfigurableConsumer
    {
        public void ConfigureSubscription(ISubscriptionBuilder builder)
        {
            builder.DeliverUsing<WorkerPoolDeliveryStrategy>(s => s.NumWorkers = 3);
        }

        public void Consume(PubSubMessage message, IDeliveryContext deliveryContext)
        {
            Console.WriteLine(message.Body);
        }
    }
}