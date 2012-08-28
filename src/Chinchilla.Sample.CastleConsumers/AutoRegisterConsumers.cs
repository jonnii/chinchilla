using Castle.Windsor;

namespace Chinchilla.Sample.CastleConsumers
{
    public class AutoRegisterConsumers : IBusConcern
    {
        private readonly WindsorContainer container;

        public AutoRegisterConsumers(WindsorContainer container)
        {
            this.container = container;
        }

        public void Run(IBus bus)
        {
            var consumers = container.ResolveAll<IConsumer>();

            foreach (var consumer in consumers)
            {
                bus.Subscribe(consumer);
            }
        }
    }
}