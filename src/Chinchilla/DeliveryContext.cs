using System;
using Chinchilla.Topologies;

namespace Chinchilla
{
    public class DeliveryContext : IDeliveryContext
    {
        public DeliveryContext(IBus bus, IDelivery delivery)
        {
            Bus = bus;
            Delivery = delivery;
        }

        public IBus Bus { get; private set; }

        public IDelivery Delivery { get; set; }

        public void Reply<TMessage>(TMessage reply)
            where TMessage : ICorrelated
        {
            if (!Delivery.IsReplyable)
            {
                var message = string.Format(
                    "Could not reply to the message with tag {0} with a message of type {1} " +
                    "because the original delivery was not replyable",
                    Delivery.Tag,
                    typeof(TMessage).Name);

                throw new ChinchillaException(message);
            }

            reply.CorrelationId = new Guid(Delivery.CorrelationId);

            var topology = new DefaultResponseTopology();

            using (var publisher = Bus.CreatePublisher<TMessage>(b => b
                .SetTopology(topology)
                .BuildTopology(false)
                .RouteWith(new DirectRouter(Delivery.ReplyTo))))
            {
                publisher.Publish(reply);
            }
        }
    }
}