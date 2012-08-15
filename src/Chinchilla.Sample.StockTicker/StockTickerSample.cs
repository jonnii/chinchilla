using System;
using System.Threading;
using Chinchilla.Sample.StockTicker.Client;
using Chinchilla.Sample.StockTicker.Server;

namespace Chinchilla.Sample.StockTicker
{
    public class StockTickerSample : IDisposable
    {
        private readonly StockTickerServer server = new StockTickerServer();

        private readonly StockTickerClient client = new StockTickerClient();

        public void Run()
        {
            var serverThread = new Thread(() => server.Start());
            serverThread.Start();

            var clientThread = new Thread(() => client.Start());
            clientThread.Start();
        }

        public void Dispose()
        {
            client.Stop();
            server.Stop();
        }
    }
}