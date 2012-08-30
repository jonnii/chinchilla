using System;
using Chinchilla.Logging;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class ErrorQueueDeliveryFailureStrategy : IDeliveryFailureStrategy, IMessageTopologyBuilder
    {
        private readonly ILogger logger = Logger.Create<ErrorQueueDeliveryFailureStrategy>();

        private readonly IPublisher<Error> publisher;

        public ErrorQueueDeliveryFailureStrategy(IBus bus)
        {
            publisher = bus.CreatePublisher<Error>(o => o.SetTopology(this));
        }

        public void Handle(IDelivery delivery, Exception exception)
        {
            logger.ErrorFormat(exception, "Error Queue is handlinge exception");

            publisher.Publish(new Error());
            delivery.Accept();
        }

        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishExchange = topology.DefineExchange("ErrorExchange", ExchangeType.Topic);

            var queue = topology.DefineQueue("ErrorQueue");
            queue.BindTo(topology.PublishExchange);

            return topology;
        }
    }
}