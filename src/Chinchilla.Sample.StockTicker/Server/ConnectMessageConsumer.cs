using System;
using Chinchilla.Sample.StockTicker.Messages;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class ConnectMessageConsumer : IConsumer<ConnectMessage>
    {
        private readonly IBus bus;

        private readonly IPublisher<PriceMessage> publisher;

        public ConnectMessageConsumer(IBus bus, IPublisher<PriceMessage> publisher)
        {
            this.bus = bus;
            this.publisher = publisher;
        }

        public void Consume(ConnectMessage message)
        {
            Console.WriteLine("Client Connected: {0} on {1}", message.ClientId, message.QueueName);

            //var exchange = publishChannel.Exchange;
            //bus.ModifyTopology((topology) =>
            //{
            //    exchange.BindTo(message.QueueName);
            //});
        }
    }
}