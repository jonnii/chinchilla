using System;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class ErrorQueueDeliveryFailureStrategy : IDeliveryFailureStrategy, IMessageTopologyBuilder
    {
        private readonly IPublisher<FailedMessage> publisher;

        public ErrorQueueDeliveryFailureStrategy(IBus bus)
        {
            publisher = bus.CreatePublisher<FailedMessage>(o => o.SetTopology(this));
        }

        public void Handle(IDelivery delivery, Exception exception)
        {
            publisher.Publish(new FailedMessage());
            delivery.Accept();
        }

        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();

            topology.PublishExchange = topology.DefineExchange("FailedMessages", ExchangeType.Topic);

            var queue = topology.DefineQueue("FailedMessages");
            queue.BindTo(topology.PublishExchange);

            return topology;
        }
    }

    public class FailedMessage
    {
    }
}