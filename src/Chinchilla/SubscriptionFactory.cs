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

            var endpointNames = configuration.EndpointNames.Any()
                ? configuration.EndpointNames
                : new[] { messageType };

            // each subscription get its own model, where each endpoint
            // will be a consumer

            var modelReferenceTag = string.Join("|", endpointNames);
            var modelReference = modelFactory.CreateModel(modelReferenceTag);

            // apply basic quality of service, this will set the prefetch count
            // which is shared across all consumers

            modelReference.Execute(
                  m => m.BasicQos(
                      configuration.PrefetchSize,
                      configuration.PrefetchCount,
                      false));

            var deliveryStrategy = configuration.BuildDeliveryStrategy(
                deliveryProcessor);

            var deliveryQueues = BuildDeliveryQueues(
                endpointNames,
                modelReference,
                messageType,
                configuration,
                faultStrategy);

            var subscription = new Subscription(
                modelReference,
                deliveryStrategy,
                deliveryQueues.ToArray());

            return Create(subscription);
        }

        /// <summary>
        /// build delivery queues for each endpoint, where each delivery queue
        /// will correspond to a basic consumer
        /// </summary>
        private IEnumerable<IDeliveryQueue> BuildDeliveryQueues(
            IEnumerable<string> endpointNames,
            IModelReference modelReference,
            string messageType,
            IEndpointConfiguration configuration,
            IFaultStrategy faultStrategy)
        {
            return endpointNames.Select((endpointName, i) =>
            {
                var endpoint = new Endpoint(endpointName, messageType, i);

                var topologyBuilder = new TopologyBuilder(modelReference);

                var topology = configuration.BuildTopology(endpoint);
                topology.Visit(topologyBuilder);

                var subscribeQueue = topology.SubscribeQueue;

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