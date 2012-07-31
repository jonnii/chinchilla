using Chinchilla.Topologies.Rabbit;

namespace Chinchilla
{
    public class SubscriptionHandle : ISubscription
    {
        public SubscriptionHandle(IQueue queue)
        {
            Queue = queue;
        }

        public IQueue Queue { get; private set; }

        public void Dispose()
        {
        }
    }
}