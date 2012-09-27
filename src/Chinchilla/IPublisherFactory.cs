using Chinchilla.Configuration;

namespace Chinchilla
{
    public interface IPublisherFactory
    {
        IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration configuration);
    }
}
