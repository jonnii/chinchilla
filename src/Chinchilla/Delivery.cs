using System;

namespace Chinchilla
{
    public class Delivery : IDelivery
    {
        private readonly IDeliveryListener listener;

        public Delivery(
            IDeliveryListener listener,
            ulong tag,
            byte[] body)
        {
            this.listener = listener;

            Tag = tag;
            Body = body;
        }

        public ulong Tag { get; private set; }

        public byte[] Body { get; private set; }

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