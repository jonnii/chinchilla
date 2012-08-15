using System;
using Chinchilla.Sample.StockTicker.Messages;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class ConnectMessageConsumer : IConsumer<ConnectMessage>
    {
        private readonly IBus bus;

        private readonly IPublishChannel publishChannel;

        public ConnectMessageConsumer(IBus bus, IPublishChannel publishChannel)
        {
            this.bus = bus;
            this.publishChannel = publishChannel;
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