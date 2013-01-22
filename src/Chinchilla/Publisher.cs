using Chinchilla.Topologies.Model;
using RabbitMQ.Client;

namespace Chinchilla
{
    public class Publisher : Trackable { }

    public class Publisher<TMessage> : Publisher, IPublisher<TMessage>
    {
        private readonly IRouter router;

        private readonly IMessageSerializer serializer;

        private bool disposed;

        public Publisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange,
            IRouter router)
        {
            this.router = Guard.NotNull(router, "router");
            this.serializer = Guard.NotNull(serializer, "serializer");

            ModelReference = Guard.NotNull(modelReference, "modelReference");
            Exchange = Guard.NotNull(exchange, "exchange");
        }

        public IModelReference ModelReference { get; private set; }

        public IExchange Exchange { get; private set; }

        public long NumPublishedMessages { get; private set; }

        public void Publish(TMessage message)
        {
            var wrappedMessage = Message.Create(message);
            var serializedMessage = serializer.Serialize(wrappedMessage);
            var defaultProperties = CreateProperties(message);
            var routingKey = router.Route(message);

            ModelReference.Execute(
                m => m.BasicPublish(
                    Exchange.Name,
                    routingKey,
                    defaultProperties,
                    serializedMessage));

            ++NumPublishedMessages;
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

            return defaultProperties;
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