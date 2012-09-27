namespace Chinchilla.Configuration
{
    public interface IPublisherConfiguration : IEndpointConfiguration
    {
        IRouter BuildRouter();

        string ExchangeName { get; }
    }
}