using System;

namespace Chinchilla.Sample.SharedSubscriptions
{
    public class SharedMessageRouter : IRouter
    {
        public string Route<TMessage>(TMessage message)
        {
            var sharedMessage = message as SharedMessage;
            if (sharedMessage == null)
            {
                throw new NotSupportedException("Cannot route =( =(");
            }

            return string.Format("messages.{0}", sharedMessage.MessageType.ToString().ToLower());
        }
    }
}