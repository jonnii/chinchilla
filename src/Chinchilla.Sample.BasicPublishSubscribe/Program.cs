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
                depot.Subscribe<PubSubMessage>(m => Console.WriteLine(m.Body), o => o.SubscribeOn("subscriber-1"));
                depot.Subscribe<PubSubMessage>(m => Console.WriteLine(m.Body), o => o.SubscribeOn("subscriber-2"));

                depot.Publish(new PubSubMessage { Body = "published!" });
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
