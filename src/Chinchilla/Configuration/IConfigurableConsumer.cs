namespace Chinchilla.Configuration
{
    /// <summary>
    /// When implemented on a consumer the consumer will be configured using
    /// the builder passed in, instead of using the default consumer subscription
    /// </summary>
    public interface IConfigurableConsumer
    {
        /// <summary>
        /// Configure the consumer
        /// </summary>
        /// <param name="builder">A subscription builder</param>
        void ConfigureSubscription(ISubscriptionBuilder builder);
    }
}
