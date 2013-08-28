using System;
using System.Collections;
using System.Collections.Generic;

namespace Chinchilla
{
    public class Delivery : IDelivery
    {
        private readonly List<IDeliveryListener> deliveryListeners = new List<IDeliveryListener>();

        public Delivery(
            ulong tag,
            byte[] body,
            string routingKey,
            string exchange,
            string contentType,
            string correlationId,
            string replyTo,
            IDictionary headers)
        {
            Tag = tag;
            Body = body;
            RoutingKey = routingKey;
            Exchange = exchange;
            ContentType = contentType;
            CorrelationId = correlationId;
            ReplyTo = replyTo;
            Headers = headers;
        }

        public ulong Tag { get; private set; }

        public byte[] Body { get; private set; }

        public string RoutingKey { get; private set; }

        public string Exchange { get; private set; }

        public string ContentType { get; private set; }

        public string CorrelationId { get; private set; }

        public string ReplyTo { get; private set; }

        public IDictionary Headers { get; private set; }

        public bool IsReplyable
        {
            get
            {
                return !string.IsNullOrEmpty(CorrelationId)
                    && !string.IsNullOrEmpty(ReplyTo);
            }
        }

        public bool HasRegisteredDeliveryListeners
        {
            get { return deliveryListeners.Count > 0; }
        }

        public void RegisterDeliveryListener(IDeliveryListener deliveryListener)
        {
            deliveryListeners.Add(deliveryListener);
        }

        public void Accept()
        {
            foreach (var listener in deliveryListeners)
            {
                listener.OnAccept(this);
            }

            deliveryListeners.Clear();
        }

        public void Reject(bool requeue)
        {
            foreach (var listener in deliveryListeners)
            {
                listener.OnReject(this, requeue);
            }

            deliveryListeners.Clear();
        }

        public void Failed(Exception e)
        {
            foreach (var listener in deliveryListeners)
            {
                listener.OnFailed(this, e);
            }

            deliveryListeners.Clear();
        }
    }
}