using System;
using System.Collections.Generic;
using System.Linq;
using Chinchilla.Configuration;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class SubscriptionFactory : TrackableFactory<Subscription>, ISubscriptionFactory
    {
        private readonly ILogger logger = Logger.Create<SubscriptionFactory>();

        private readonly IModelFactory modelFactory;

        private readonly IMessageSerializer messageSerializer;

        public SubscriptionFactory(
            IModelFactory modelFactory,
            IMessageSerializer messageSerializer)
        {
            this.modelFactory = modelFactory;
            this.messageSerializer = messageSerializer;
        }

        public ISubscription Create<TMessage>(
            IBus bus,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback)
        {
            logger.DebugFormat("Creating new handler subscription with configuration: {0}", configuration);

            var deliveryProcessor = new ActionDeliveryProcessor<TMessage>(
                bus,
                messageSerializer,
                callback);

            var messageType = typeof(TMessage).Name;

            var faultStrategy = configuration.BuildFaultStrategy(bus);

            var deliveryQueues = GetSubscriptionQueuesForEndpoints(
                messageType,
                configuration,
                faultStrategy);

            var deliveryStrategy = configuration.BuildDeliveryStrategy(deliveryProcessor);
            var subscription = new Subscription(
                deliveryStrategy,
                deliveryQueues.ToArray());

            return Create(subscription);
        }

        private IEnumerable<IDeliveryQueue> GetSubscriptionQueuesForEndpoints(
            string messageType,
            ISubscriptionConfiguration configuration,
            IFaultStrategy faultStrategy)
        {
            var endpointNames = configuration.EndpointNames.Any()
                ? configuration.EndpointNames
                : new[] { messageType };

            return endpointNames.Select((endpointName, i) =>
            {
                var modelReference = modelFactory.CreateModel(endpointName);

                var endpoint = new Endpoint(endpointName, messageType, i);

                var topologyBuilder = new TopologyBuilder(modelReference);

                var topology = configuration.BuildTopology(endpoint);
                topology.Visit(topologyBuilder);

                var subscribeQueue = topology.SubscribeQueue;

                modelReference.Execute(
                    m => m.BasicQos(
                        configuration.PrefetchSize,
                        configuration.PrefetchCount,
                        false));

                return new DeliveryQueue(
                    subscribeQueue, modelReference, faultStrategy);
            });
        }

        public ISubscription Create(
            Subscription subscription)
        {
            Track(subscription);
            return subscription;
        }

        public override void Dispose()
        {
            logger.Debug("Disposing of subscription factory");

            base.Dispose();
        }
    }
}