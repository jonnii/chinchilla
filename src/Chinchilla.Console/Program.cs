using System.Threading;
using Chinchilla.Logging;

namespace Chinchilla.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var running = true;

            Logger.Factory = new ConsoleLoggerFactory();

            var publisher = new Thread(() =>
            {
                using (var bus = Depot.Connect("localhost"))
                {
                    while (running)
                    {
                        bus.Publish(new HelloWorldMessage
                        {
                            Message = "Good morning"
                        });

                        Thread.Sleep(1000);
                    }
                }
            });
            publisher.Start();

            //var consumer = new Thread(() =>
            //{
            //    using (var bus = Depot.Connect("localhost"))
            //    {
            //        bus.Subscribe<HelloWorldMessage>(m => System.Console.WriteLine(m.Message));

            //        while (running)
            //        {
            //            Thread.Sleep(1000);
            //        }
            //    }
            //});
            //consumer.Start();

            System.Console.WriteLine("Waiting for you!");
            System.Console.ReadLine();

            running = false;
        }
    }

    public class HelloWorldMessage
    {
        public string Message { get; set; }
    }
}
