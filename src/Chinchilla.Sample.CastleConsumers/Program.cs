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
            Logger.Factory = new ConsoleLoggerFactory();

            using (var container = new WindsorContainer())
            {
                var currentContainer = container;

                container.Register(
                    Classes.FromThisAssembly().BasedOn(typeof(IConsumer)).WithServiceAllInterfaces(),
                    Component.For<IBus>().UsingFactoryMethod(() =>
                    {
                        var settings = new DepotSettings
                        {
                            ConsumerFactoryBuilder = () => new WindsorConsumerFactory(currentContainer)
                        };

                        settings.AddStartupConcern(new AutoRegisterConsumers(currentContainer));

                        return Depot.Connect("localhost/castle_consumer", settings);
                    }));

                var bus = container.Resolve<IBus>();

                bus.Publish(new PubSubMessage { Body = "published!" });

                Console.WriteLine("Finished press any key to close");
                Console.ReadKey();
            }
        }
    }
}
