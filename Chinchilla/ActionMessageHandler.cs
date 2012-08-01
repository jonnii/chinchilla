using System;

namespace Chinchilla
{
    public class ActionMessageHandler<T> : IMessageHandler<T>
    {
        private readonly Action<T> handler;

        public ActionMessageHandler(Action<T> handler)
        {
            this.handler = handler;
        }

        public void Handle(IMessage<T> message)
        {
            var body = message.Body;
            handler(body);
        }
    }
}