using System;
using Chinchilla.Integration.Features.Messages;

namespace Chinchilla.Integration.Features.Consumers
{
    public class HelloWorldMessageConsumer : IConsumer<HelloWorldMessage>
    {
        public void Consume(HelloWorldMessage message, IMessageContext messageContext)
        {
            Console.WriteLine("consuming message!");
        }
    }
}
