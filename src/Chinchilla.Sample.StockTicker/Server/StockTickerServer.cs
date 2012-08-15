using System.Threading;
using Chinchilla.Sample.StockTicker.Messages;

namespace Chinchilla.Sample.StockTicker.Server
{
    public class StockTickerServer
    {
        private readonly IBus bus;

        private bool isRunning;

        private ISubscription connectMessageSubscription;

        public StockTickerServer()
        {
            bus = Depot.Connect("localhost/ticker");
        }

        public void Start()
        {
            isRunning = true;

            using (var publishChannel = bus.CreatePublisher<PriceMessage>(ConfigurePricePublisher))
            {
                connectMessageSubscription = bus.Subscribe(
                    new ConnectMessageConsumer(bus, publishChannel));

                while (isRunning)
                {
                    publishChannel.Publish(new PriceMessage("GOOG", 300));

                    // publish updates
                    Thread.Sleep(1000);
                }
            }
        }

        public void Stop()
        {
            connectMessageSubscription.Dispose();
            bus.Dispose();
            isRunning = false;
        }

        private void ConfigurePricePublisher(IPublisherBuilder builder)
        {
            builder.SetTopology(new PricePublisherTopology());
        }
    }
}