using Chinchilla.Integration.Features.Messages;

namespace Chinchilla.Integration.Features.Consumers
{
    class CapitalizeMessageConsumer : IConsumer<CapitalizeMessage>
    {
        public void Consume(CapitalizeMessage message, IDeliveryContext deliveryContext)
        {
            var upcased = message.Word.ToUpper();

            deliveryContext.Reply(new CapitalizedMessage(upcased));
        }
    }
}
