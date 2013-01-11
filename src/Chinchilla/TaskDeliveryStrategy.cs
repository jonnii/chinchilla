using System.Threading.Tasks;

namespace Chinchilla
{
    public class TaskDeliveryStrategy : DeliveryStrategy
    {
        public override void Deliver(IDelivery delivery)
        {
            DeliverOnTask(delivery);
        }

        public override DeliveryStrategyState GetState()
        {
            return new DeliveryStrategyState();
        }

        public Task DeliverOnTask(IDelivery delivery)
        {
            var currentDelivery = delivery;

            return Task.Factory.StartNew(() => connectedProcessor.Process(delivery))
                .ContinueWith(
                t =>
                {
                    if (t.IsFaulted)
                    {
                        currentDelivery.Failed(t.Exception);
                    }
                    else
                    {
                        currentDelivery.Accept();
                    }
                });
        }
    }
}