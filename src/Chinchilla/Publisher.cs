using Chinchilla.Topologies.Model;

namespace Chinchilla
{
    public class Publisher<TMessage> : IPublisher<TMessage>
    {
        private readonly IMessageSerializer serializer;

        private bool disposed;

        public Publisher(
            IModelReference modelReference,
            IMessageSerializer serializer,
            IExchange exchange)
        {
            this.serializer = Guard.NotNull(serializer, "serializer");

            ModelReference = Guard.NotNull(modelReference, "modelReference");
            Exchange = Guard.NotNull(exchange, "exchange");
        }

        public IModelReference ModelReference { get; private set; }

        public IExchange Exchange { get; private set; }

        public long NumPublishedMessages { get; private set; }

        public void Publish(TMessage message)
        {
            var defaultProperties = ModelReference.Execute(m => m.CreateBasicProperties());

            var serializedMessage = serializer.Serialize(
                Message.Create(message));

            var hasRoutingKey = message as IHasRoutingKey;
            var routingKey = hasRoutingKey != null ? hasRoutingKey.RoutingKey : "#";

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