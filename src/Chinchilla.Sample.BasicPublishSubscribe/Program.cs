using System;
using Chinchilla.Logging;

namespace Chinchilla.Sample.BasicPublishSubscribe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Target = new ConsoleLogger();

            using (var depot = Depot.Connect("localhost/samplepubsub"))
            {
                depot.Subscribe<PubSubMessage>(m => Console.WriteLine(m.Body));
            }

            Console.WriteLine("Finished press any key to close");
            Console.ReadKey();
        }
    }

    public class PubSubMessage
    {
        public string Body { get; set; }
    }
}
