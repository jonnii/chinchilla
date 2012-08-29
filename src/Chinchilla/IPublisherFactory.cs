using Chinchilla.Configuration;
using Chinchilla.Serializers;

namespace Chinchilla
{
    public interface IPublisherFactory
    {
        IPublisher<TMessage> Create<TMessage>(
            IModelReference modelReference,
            IPublisherConfiguration configuration);
    }
}
