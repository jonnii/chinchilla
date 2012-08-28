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
                var currentContainer = container;

                container.Register(
                    AllTypes.FromThisAssembly().BasedOn(typeof(IConsumer)),
                    Component.For<IBus>().UsingFactoryMethod(() =>
                    {
                        var settings = new DepotSettings
                        {
                            ConsumerFactoryBuilder = () => new WindsorConsumerFactory(currentContainer)
                        };

                        return Depot.Connect("localhost/samplepubsub", settings);
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
}
