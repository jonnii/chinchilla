using System;

namespace Chinchilla.Sample.CastleConsumers
{
    public class PubSubConsumer : IConsumer<PubSubMessage>
    {
        public void Consume(PubSubMessage message, IMessageContext messageContext)
        {
            Console.WriteLine(message.Body);
        }
    }
}