namespace Chinchilla.Integration.Features.Messages
{
    public class HelloWorldMessage : IHasRoutingKey, IHelloWorldMessage
    {
        public string Message { get; set; }

        string IHasRoutingKey.RoutingKey
        {
            get { return "messages." + Message; }
        }
    }
}