using System;
using System.Linq;
using Chinchilla.Sample.StockTicker.Messages;

namespace Chinchilla.Sample.StockTicker.Client
{
    public class StockTickerClient
    {
        private readonly IBus bus;

        private readonly string id;

        private ISubscription priceSubscription;

        public StockTickerClient()
        {
            id = Guid.NewGuid().ToString();
            bus = Depot.Connect("localhost/ticker");
        }

        public void Start()
        {
            priceSubscription = bus.Subscribe<PriceMessage>(OnPrice, ConfigurePriceSubscription);

            var binding = priceSubscription.Queue.Bindings.First();

            bus.Publish(new ConnectMessage(
                id,
                binding.Exchange.Name,
                new[] { "GOOG" }));
        }

        public void Stop()
        {
            priceSubscription.Dispose();
            bus.Dispose();
        }

        private void OnPrice(PriceMessage price)
        {
            Console.WriteLine("RECEIVED: {0} = {1}", price.Ticker, price.Price);
        }

        private void ConfigurePriceSubscription(ISubscriptionBuilder subscriptionConfiguration)
        {
            subscriptionConfiguration.SetTopology(
                messageType => new PriceSubscriberTopology(id));
        }
    }
}