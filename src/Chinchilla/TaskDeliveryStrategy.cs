using System.Threading.Tasks;

namespace Chinchilla
{
    public class TaskDeliveryStrategy : DeliveryStrategy
    {
        public override void Deliver(IDelivery delivery)
        {
            var currentDelivery = delivery;

            Task.Factory.StartNew(() => connectedProcessor.Process(delivery))
                .ContinueWith(_ => currentDelivery.Accept());
        }
    }
}