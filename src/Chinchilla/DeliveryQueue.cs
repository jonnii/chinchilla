using System;
using System.Collections.Concurrent;
using System.Threading;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class DeliveryQueue : IDeliveryQueue
    {
        private const int ConsumerTakeTimeoutInMs = 10;

        private readonly IQueue queue;

        private readonly IModelReference modelReference;

        private readonly ISubscriptionFailureStrategy subscriptionFailureStrategy;

        private BlockingCollection<BasicDeliverEventArgs> consumerQueue;

        private long numAcceptedMessages;

        private long numRejectedMessages;

        private long numFailedMessages;

        public DeliveryQueue(
            IQueue queue,
            IModelReference modelReference,
            ISubscriptionFailureStrategy subscriptionFailureStrategy)
        {
            this.queue = queue;
            this.modelReference = modelReference;
            this.subscriptionFailureStrategy = subscriptionFailureStrategy;
        }

        public string Name
        {
            get { return queue.Name; }
        }

        public long NumAcceptedMessages
        {
            get { return numAcceptedMessages; }
        }

        public long NumRejectedMessages
        {
            get { return numRejectedMessages; }
        }

        public long NumFailedMessages
        {
            get { return numFailedMessages; }
        }

        public void OnAccept(IDelivery delivery)
        {
            Interlocked.Increment(ref numAcceptedMessages);

            modelReference.Execute(
                m => m.BasicAck(delivery.Tag, false));
        }

        public void OnReject(IDelivery delivery, bool requeue)
        {
            Interlocked.Increment(ref numRejectedMessages);

            modelReference.Execute(
                m => m.BasicNack(delivery.Tag, false, requeue));
        }

        public void OnFailed(IDelivery delivery, Exception exception)
        {
            subscriptionFailureStrategy.OnFailure(delivery, exception);

            Interlocked.Increment(ref numFailedMessages);
        }

        public bool TryTake(out BasicDeliverEventArgs item)
        {
            if (consumerQueue == null)
            {
                throw new InvalidOperationException(
                    "Cannot take out of the consumer queue until the delivery has been started, " +
                    "did you call Start?");
            }

            return consumerQueue.TryTake(out item, ConsumerTakeTimeoutInMs);
        }

        public void Start()
        {
            consumerQueue = modelReference.GetConsumerQueue(queue);
        }

        public QueueState GetState()
        {
            return new QueueState(Name, NumAcceptedMessages, numRejectedMessages, NumFailedMessages);
        }

        public override string ToString()
        {
            return string.Format("[DeliveryQueue Name={0}]", Name);
        }
    }
}