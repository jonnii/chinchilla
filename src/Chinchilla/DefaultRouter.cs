namespace Chinchilla
{
    public class DefaultRouter : IRouter
    {
        private readonly string replyTo;

        public DefaultRouter()
            : this(string.Empty)
        {

        }

        public DefaultRouter(string replyTo)
        {
            this.replyTo = replyTo;
        }

        public virtual string Route<TMessage>(TMessage message)
        {
            var hasRoutingKey = message as IHasRoutingKey;

            return hasRoutingKey != null
                ? hasRoutingKey.RoutingKey
                : "#";
        }

        public virtual string ReplyTo()
        {
            return replyTo;
        }
    }
}