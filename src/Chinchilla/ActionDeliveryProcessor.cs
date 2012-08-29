using System;

namespace Chinchilla
{
    /// <summary>
    /// The action delivery processor takes a delivery and unwraps it as a
    /// typed message. The body of this message is then handed to an action 
    /// callback.
    /// </summary>
    /// <typeparam name="T">The type of message to be delivered to the action</typeparam>
    public class ActionDeliveryProcessor<T> : IDeliveryProcessor
    {
        private readonly IBus bus;

        private readonly IMessageSerializer messageSerializer;

        private readonly Action<T, IDeliveryContext> handler;

        public ActionDeliveryProcessor(
            IBus bus,
            IMessageSerializer messageSerializer,
            Action<T, IDeliveryContext> handler)
        {
            this.bus = bus;
            this.messageSerializer = messageSerializer;
            this.handler = handler;
        }

        public void Process(IDelivery delivery)
        {
            var deserialized = messageSerializer.Deserialize<T>(delivery.Body);
            var deliveryContext = new DeliveryContext(bus);

            handler(deserialized.Body, deliveryContext);
        }
    }
}