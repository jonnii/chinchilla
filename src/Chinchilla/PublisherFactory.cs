namespace Chinchilla
{
    public class PublisherFactory : IPublisherFactory
    {
        private readonly IMessageSerializer messageSerializer;

        public PublisherFactory(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration configuration)
        {
            return new Publisher<TMessage>(
                modelReference, messageSerializer);
        }
    }
}