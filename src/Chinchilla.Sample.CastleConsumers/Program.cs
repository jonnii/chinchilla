using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Chinchilla.Logging;

namespace Chinchilla.Sample.CastleConsumers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.Target = new ConsoleLogger();

            using (var container = new WindsorContainer())
            {
                container.Register(
                    AllTypes.FromThisAssembly().BasedOn(typeof(IConsumer<>)),
                    Component.For<IBus>().UsingFactoryMethod(() =>
                    {
                        return Depot.Connect("localhost/samplepubsub");
                    }));

                var bus = container.Resolve<IBus>();

                bus.Publish(new PubSubMessage
                {
                    Body = "published!"
                });

                Console.WriteLine("Finished press any key to close");
                Console.ReadKey();
            }
        }
    }

    public class PubSubMessage
    {
        public string Body { get; set; }
    }
}
