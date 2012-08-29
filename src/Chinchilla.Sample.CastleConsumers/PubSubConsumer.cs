using System;

namespace Chinchilla.Sample.CastleConsumers
{
    public class PubSubConsumer : IConsumer<PubSubMessage>
    {
        public void Consume(PubSubMessage message, IDeliveryContext deliveryContext)
        {
            Console.WriteLine(message.Body);
        }
    }
}