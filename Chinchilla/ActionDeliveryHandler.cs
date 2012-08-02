using System;

namespace Chinchilla
{
    public class ActionDeliveryHandler<T> : IDeliveryHandler
    {
        private readonly IMessageSerializer messageSerializer;

        private readonly Action<T> handler;

        public ActionDeliveryHandler(
            IMessageSerializer messageSerializer,
            Action<T> handler)
        {
            this.messageSerializer = messageSerializer;
            this.handler = handler;
        }

        public void Handle(IDelivery delivery)
        {
            var deserialized = messageSerializer.Deserialize<T>(delivery.Body);
            handler(deserialized.Body);
        }
    }
}