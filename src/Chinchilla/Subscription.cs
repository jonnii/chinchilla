using System;
using System.Linq;
using System.Threading;
using Chinchilla.Logging;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class Subscription : Trackable, ISubscription
    {
        private readonly ILogger logger = Logger.Create<Subscription>();

        private readonly IModelReference modelReference;

        private readonly IDeliveryStrategy deliveryStrategy;

        private Thread listenerThread;

        private bool isDisposing;

        public Subscription(
            string name,
            IModelReference modelReference,
            IDeliveryStrategy deliveryStrategy,
            IDeliveryQueue[] queues)
        {
            this.modelReference = modelReference;
            this.deliveryStrategy = deliveryStrategy;

            Name = name;
            Queues = queues;
        }

        public string Name { get; private set; }

        public IDeliveryQueue[] Queues { get; private set; }

        public bool IsStarted { get; private set; }

        public bool IsStartable
        {
            get { return deliveryStrategy.IsStartable; }
        }

        public SubscriptionState State
        {
            get
            {
                var queueStates = Queues.Select(q => q.GetState()).ToArray();
                var workerStates = deliveryStrategy.GetWorkerStates();

                return new SubscriptionState(
                    Name,
                    IsStarted,
                    IsStartable,
                    queueStates,
                    workerStates);
            }
        }

        public IWorkersController Workers
        {
            get { return deliveryStrategy.GetWorkersController(); }
        }

        public void Start()
        {
            logger.InfoFormat("Starting subscription: {0}", this);

            logger.Debug(" -> Building listener thread");
            listenerThread = BuildListenerThread();

            logger.Debug(" -> starting delivery strategy");
            deliveryStrategy.Start();

            logger.Debug(" -> starting all queues");
            foreach (var queue in Queues)
            {
                queue.Start();
            }

            logger.Debug(" -> Starting listener thread");
            listenerThread.Start();

            IsStarted = true;
        }

        private Thread BuildListenerThread()
        {
            return new Thread(() =>
            {
                while (!isDisposing)
                {
                    BasicDeliverEventArgs item = null;

                    // grab the first item where any of the consumer queues has a value

                    var queue = Queues
                        .FirstOrDefault(q => q.TryTake(out item));

                    // if we're dead and we didn't find an item, 
                    // it means it's time to pack up and go home

                    if (isDisposing && queue == null)
                    {
                        break;
                    }

                    // if we're not dead and we didn't find an item
                    // lets go around again

                    if (!isDisposing && queue == null)
                    {
                        continue;
                    }

                    if (item == null)
                    {
                        continue;
                    }

                    var delivery = new Delivery(
                        item.DeliveryTag,
                        item.Body,
                        item.RoutingKey,
                        item.Exchange,
                        item.BasicProperties.ContentType,
                        item.BasicProperties.CorrelationId,
                        item.BasicProperties.ReplyTo);

                    // register the queue as a delivery listener on the delivery so that 
                    // we can ack or nack the message depending on whether or not it was processed
                    delivery.RegisterDeliveryListener(queue);

                    // deliver the message to the delivery strategy
                    deliveryStrategy.Deliver(delivery);
                }

                logger.DebugFormat("Subscription thread terminated for: {0}", this);
            });
        }

        public override void Dispose()
        {
            logger.DebugFormat("Shutting down subscription: {0}", this);

            if (isDisposing)
            {
                var message = string.Format("Subscription already disposing: {0}", this);
                throw new ObjectDisposedException(message);
            }

            // 1. we're disposing, let everyone know

            isDisposing = true;

            // 2. tell the channel to stop receiving new messages

            logger.Info("SHUTDOWN: Stopping channel flow");
            modelReference.Execute(m => m.ChannelFlow(false));

            // 3. we've disposed of the current subscription so wait for the 
            //    listener thread to to finish doing what it's doing, it might
            //    dispatch something to the delivery strategy. once we're done
            //    with this then we'll stop accepting any new messages

            logger.Info("SHUTDOWN: Waiting for listener thread to terminate");
            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join();
            }

            // 4. stop the delivery strategy, this will wait for any work currently
            //    running to finish

            logger.Info("SHUTDOWN: Stopping delivery strategy");
            deliveryStrategy.Stop();

            // 5. finally we can dispose of the model reference, this will close
            //    the model and cancel any consumers that are outstanding

            logger.Info("SHUTDOWN: disposing of model reference");
            modelReference.Dispose();

            base.Dispose();
        }

        public override string ToString()
        {
            var queueNames = string.Join(",", Queues.Select(q => q.ToString()));
            return string.Format("[Subscription Queues={0}]", queueNames);
        }
    }
}
