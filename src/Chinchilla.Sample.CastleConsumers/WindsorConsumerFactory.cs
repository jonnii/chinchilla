using Castle.Windsor;

namespace Chinchilla.Sample.CastleConsumers
{
    public class WindsorConsumerFactory : IConsumerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorConsumerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public IConsumer Build<T>()
            where T : IConsumer
        {
            return container.Resolve<T>();
        }

        public void Dispose()
        {

        }
    }
}