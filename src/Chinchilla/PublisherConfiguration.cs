namespace Chinchilla
{
    public class PublisherConfiguration : IPublisherConfiguration, IPublisherBuilder
    {
        public static PublisherConfiguration Default
        {
            get { return new PublisherConfiguration(); }
        }
    }
}