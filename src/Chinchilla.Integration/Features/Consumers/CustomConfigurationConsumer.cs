using Chinchilla.Configuration;
using Chinchilla.Integration.Features.Messages;

namespace Chinchilla.Integration.Features.Consumers
{
    public class CustomConfigurationConsumer : IConsumer<HelloWorldMessage>, IConfigurableConsumer
    {
        public void ConfigureSubscription(ISubscriptionBuilder builder)
        {
            builder.SubscribeOn("custom-subscription-endpoint");
        }

        public void Consume(HelloWorldMessage message, IDeliveryContext deliveryContext)
        {

        }
    }
}
