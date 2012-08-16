using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class Publisher<TMessage> : IPublisher<TMessage>
    {
        private readonly IModelReference modelReference;

        private readonly IMessageSerializer serializer;

        private bool disposed;

        public Publisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange)
        {
            this.modelReference = Guard.NotNull(modelReference, "modelReference");
            this.serializer = Guard.NotNull(serializer, "serializer");
            Exchange = Guard.NotNull(exchange, "exchange");
        }

        public IExchange Exchange { get; private set; }

        public long NumPublishedMessages { get; private set; }

        public void Publish(TMessage message)
        {
            var defaultProperties = modelReference.Execute(m => m.CreateBasicProperties());

            var serializedMessage = serializer.Serialize(
                Message.Create(message));

            var hasRoutingKey = message as IHasRoutingKey;
            var routingKey = hasRoutingKey != null ? hasRoutingKey.RoutingKey : "#";

            modelReference.Execute(
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

            modelReference.Dispose();

            disposed = true;
        }
    }
}