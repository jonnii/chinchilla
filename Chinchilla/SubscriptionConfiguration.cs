namespace Chinchilla
{
    public class SubscriptionConfiguration : ISubscriptionConfiguration
    {
        public static ISubscriptionConfiguration Default
        {
            get { return new SubscriptionConfiguration(); }
        }

        public ISubscriptionConfiguration MaxConsumers(int maxConsumers)
        {
            return this;
        }

        public override string ToString()
        {
            return string.Format("[SubscriptionConfiguration]");
        }
    }
}