using System;

namespace Chinchilla
{
    public class Delivery : IDelivery
    {
        private readonly IDeliveryListener listener;

        public Delivery(
            IDeliveryListener listener,
            ulong tag,
            byte[] body,
            string routingKey,
            string exchange,
            string contentType,
            string correlationId,
            string replyTo)
        {
            this.listener = listener;

            Tag = tag;
            Body = body;
            RoutingKey = routingKey;
            Exchange = exchange;
            ContentType = contentType;
            CorrelationId = correlationId;
            ReplyTo = replyTo;
        }

        public ulong Tag { get; private set; }

        public byte[] Body { get; private set; }

        public string RoutingKey { get; private set; }

        public string Exchange { get; private set; }

        public string ContentType { get; private set; }

        public string CorrelationId { get; private set; }

        public string ReplyTo { get; private set; }

        public bool IsReplyable
        {
            get
            {
                return !string.IsNullOrEmpty(CorrelationId)
                    && !string.IsNullOrEmpty(ReplyTo);
            }
        }

        public void Accept()
        {
            listener.OnAccept(this);
        }

        public void Failed(Exception e)
        {
            listener.OnFailed(this, e);
        }
    }
}