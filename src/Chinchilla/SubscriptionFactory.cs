using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Chinchilla.Configuration;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class SubscriptionFactory : TrackableFactory<Subscription>, ISubscriptionFactory
    {
        private readonly ILogger logger = Logger.Create<SubscriptionFactory>();

        private readonly IModelFactory modelFactory;

        private readonly IMessageSerializers messageSerializers;

        public SubscriptionFactory(
            IModelFactory modelFactory,
            IMessageSerializers messageSerializers)
        {
            this.modelFactory = modelFactory;
            this.messageSerializers = messageSerializers;
        }

        public IEnumerable<ISubscription> List()
        {
            return Tracked;
        }

        public string GetMessageTypeName(Type type)
        {
            var typeName = type.Name;
            
            if (!type.GetTypeInfo().IsInterface)
            {
                return typeName;
            }

            if (!typeName.StartsWith("I"))
            {
                var message = string.Format(
                    "Cannot use '{0}' as an interface message contract because the message type name doesn't start with 'I'",
                    typeName);

                throw new NotSupportedException(message);
            }

            return typeName.Substring(1);
        }

        public ISubscription Create<TMessage>(
            IBus bus,
            ISubscriptionConfiguration configuration,
            Action<TMessage, IDeliveryContext> callback)
        {
            logger.DebugFormat("Creating new handler subscription with configuration: {0}", configuration);

            var deliveryProcessor = new ActionDeliveryProcessor<TMessage>(
                bus,
                messageSerializers,
                callback);

            var messageType = GetMessageTypeName(typeof(TMessage));

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
                configuration.Name,
                modelReference,
                deliveryStrategy,
                deliveryQueues.ToArray());

            return Create(subscription);
        }

        public ISubscription FindByName(string name)
        {
            var subscription = Tracked.FirstOrDefault(s => s.Name == name);

            if (subscription == null)
            {
                var message = string.Format(
                    "Could not find a subscription with the name: {0}", name);

                throw new ChinchillaException(message);
            }

            return subscription;
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
            ISubscriptionFailureStrategy subscriptionFailureStrategy)
        {
            return endpointNames.Select((endpointName, i) =>
            {
                var endpoint = new Endpoint(endpointName, messageType, i);

                var topologyBuilder = new TopologyBuilder(modelReference);

                var topology = configuration.BuildTopology(endpoint);
                topology.Visit(topologyBuilder);

                var subscribeQueue = topology.SubscribeQueue;

                return new DeliveryQueue(
                    subscribeQueue, modelReference, subscriptionFailureStrategy);
            });
        }

        public ISubscription Create(Subscription subscription)
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