using System;
using Chinchilla.Logging;

namespace Chinchilla.Sample.Timeouts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting program");

            Logger.Factory = new ConsoleLoggerFactory();

            using (var sample = new TimeoutSample())
            {
                Console.WriteLine("Press any key to stop...");
                sample.Run();
                Console.ReadKey();
            }
        }
    }
}
