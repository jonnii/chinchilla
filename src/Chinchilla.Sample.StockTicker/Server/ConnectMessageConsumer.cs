using System;
using System.Linq;
using Chinchilla.Sample.StockTicker.Messages;
using Chinchilla.Topologies.Model;

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
            var exchange = publisher.Exchange;

            var keys = message.Tickers.Select(
                t => string.Format("prices.{0}", t)).ToArray();

            var formattedKeys = string.Join(", ", keys);
            Console.WriteLine("Client Connected: {0} on {1} for {2}", message.ClientId, message.QueueName, formattedKeys);

            var binding = new Binding(
                new Exchange(exchange.Name, ExchangeType.Topic),
                new Exchange(message.QueueName, ExchangeType.Topic),
                keys);

            var topologyBuilder = new TopologyBuilder(publisher.ModelReference);
            topologyBuilder.Visit(binding);
        }
    }
}