using System;

namespace Chinchilla.Sample.CastleConsumers
{
    public class PubSubConsumer : IConsumer<PubSubMessage>
    {
        public void Consume(PubSubMessage message)
        {
            Console.WriteLine(message.Body);
        }
    }
}