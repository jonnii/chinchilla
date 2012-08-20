namespace Chinchilla
{
    public class DefaultRouter : IRouter
    {
        public string Route<TMessage>(TMessage message)
        {
            var hasRoutingKey = message as IHasRoutingKey;

            return hasRoutingKey != null
                ? hasRoutingKey.RoutingKey
                : "#";
        }
    }
}