using System;

namespace Chinchilla
{
    /// <summary>
    /// The action delivery processor takes a delivery and unwraps it as a
    /// typed message. The body of this message is then handed to an action 
    /// callback.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionDeliveryProcessor<T> : IDeliveryProcessor
    {
        private readonly IMessageSerializer messageSerializer;

        private readonly Action<T, IMessageContext> handler;

        public ActionDeliveryProcessor(
            IMessageSerializer messageSerializer,
            Action<T, IMessageContext> handler)
        {
            this.messageSerializer = messageSerializer;
            this.handler = handler;
        }

        public void Process(IDelivery delivery)
        {
            var deserialized = messageSerializer.Deserialize<T>(delivery.Body);

            handler(deserialized.Body, new MessageContext());
        }
    }
}