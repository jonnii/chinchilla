using System;
using System.Threading;
using Chinchilla.Logging;
using Chinchilla.Topologies.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla
{
    public class Publisher : Trackable { }

    public class Publisher<TMessage> : Publisher, IPublisher<TMessage>
    {
        private readonly ILogger logger = Logger.Create<Publisher<TMessage>>();

        private readonly IRouter router;

        private readonly IMessageSerializer serializer;

        protected bool disposed;

        private long numPublishedMessages;

        public Publisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange,
            IRouter router)
        {
            this.router = Guard.NotNull(router, "router");
            this.serializer = Guard.NotNull(serializer, "serializer");

            ModelReference = Guard.NotNull(modelReference, "modelReference");
            Exchange = Guard.NotNull(exchange, "bindable");
        }

        public IModelReference ModelReference { get; private set; }

        public IExchange Exchange { get; private set; }

        public long NumPublishedMessages
        {
            get { return numPublishedMessages; }
        }

        public virtual void Start()
        {

        }

        public IPublishReceipt Publish(TMessage message)
        {
            var wrappedMessage = Message.Create(message);
            var serializedMessage = serializer.Serialize(wrappedMessage);
            var defaultProperties = CreateProperties(message);
            var routingKey = router.Route(message);

            var publishReceipt = ModelReference.Execute(
                model => PublishWithReceipt(
                    model,
                    routingKey,
                    defaultProperties,
                    serializedMessage));

            Interlocked.Increment(ref numPublishedMessages);

            return publishReceipt;
        }

        public virtual IPublishReceipt PublishWithReceipt(
            IModel model,
            string routingKey,
            IBasicProperties defaultProperties,
            byte[] serializedMessage)
        {
            model.BasicPublish(
                Exchange.Name,
                routingKey,
                defaultProperties,
                serializedMessage);

            return NullReceipt.Instance;
        }

        public IBasicProperties CreateProperties(TMessage message)
        {
            var defaultProperties = ModelReference.Execute(m => m.CreateBasicProperties());
            defaultProperties.ContentType = serializer.ContentType;

            var correlated = message as ICorrelated;
            if (correlated != null)
            {
                defaultProperties.CorrelationId = correlated.CorrelationId.ToString();
            }

            var replyTo = router.ReplyTo();
            if (!string.IsNullOrEmpty(replyTo))
            {
                defaultProperties.ReplyTo = router.ReplyTo();
            }

            return defaultProperties;
        }

        public void EnableConfirms()
        {
            logger.InfoFormat("Enabling publisher confirms on {0}", this);

            //ModelReference.OnReconnect();

            ModelReference.Execute(m => m.ConfirmSelect());
            ModelReference.Execute(m => m.BasicAcks += delegate(IModel model, BasicAckEventArgs args)
            {
                Console.WriteLine(args.DeliveryTag);
            });
        }

        public override void Dispose()
        {
            if (disposed)
            {
                return;
            }

            ModelReference.Dispose();
            disposed = true;
        }
    }
}