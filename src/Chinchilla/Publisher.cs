using Chinchilla.Topologies.Model;

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

            var defaultProperties = ModelReference.Execute(m => m.CreateBasicProperties());
            defaultProperties.ContentType = serializer.ContentType;

            var routingKey = router.Route(message);

            ModelReference.Execute(
                m => m.BasicPublish(
                    Exchange.Name,
                    routingKey,
                    defaultProperties,
                    serializedMessage));

            ++NumPublishedMessages;
        }

        public void Dispose()
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