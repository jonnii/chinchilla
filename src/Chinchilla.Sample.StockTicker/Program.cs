using System;
using Chinchilla.Logging;

namespace Chinchilla.Sample.StockTicker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting program");

            Logger.Factory = new ConsoleLoggerFactory();

            using (var sample = new StockTickerSample())
            {
                Console.WriteLine("Press any key to stop...");
                sample.Run();
                Console.ReadKey();
            }
        }
    }
}
