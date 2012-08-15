using System;
using Chinchilla.Logging;

namespace Chinchilla.Sample.StockTicker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting program");

            Logger.Target = new ConsoleLogger();

            using (var sample = new StockTickerSample())
            {
                Console.WriteLine("Press any key to stop...");
                sample.Run();
                Console.ReadKey();
            }
        }
    }
}
