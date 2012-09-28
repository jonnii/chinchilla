using System;
using System.Text;
using Chinchilla.Logging;
using Chinchilla.Topologies;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class ErrorQueueFaultStrategy : IFaultStrategy, IMessageTopologyBuilder
    {
        private readonly ILogger logger = Logger.Create<ErrorQueueFaultStrategy>();

        private readonly Lazy<IPublisher<Fault>> publisher;

        public ErrorQueueFaultStrategy(IBus bus)
        {
            publisher = new Lazy<IPublisher<Fault>>(
                () => bus.CreatePublisher<Fault>(o => o.SetTopology(this)));
        }

        public void ProcessFailedDelivery(IDelivery delivery, Exception exception)
        {
            logger.ErrorFormat(exception, "Error Queue is handlinge exception");

            var error = BuildFault(delivery, exception);
            publisher.Value.Publish(error);

            delivery.Accept();
        }

        public Fault BuildFault(IDelivery delivery, Exception exception)
        {
            var messageAsString = Encoding.UTF8.GetString(delivery.Body);

            return new Fault
            {
                RoutingKey = delivery.RoutingKey,
                Exchange = delivery.Exchange,
                FaultedMessage = messageAsString,
                Exception = new FaultException
                {
                    Message = exception.Message,
                    Type = exception.GetType().FullName,
                    StackTrace = exception.StackTrace
                }
            };
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