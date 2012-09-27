using System;
using Chinchilla.Logging;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting program");

            Logger.Target = new ConsoleLogger();

            using (var sample = new SharedSubscriptionSample())
            {
                Console.WriteLine("Press any key to stop...");
                sample.Run();
                Console.ReadKey();
            }
        }
    }
}
