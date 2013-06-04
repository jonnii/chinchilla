namespace Chinchilla.Integration.Features.Messages
{
    public class HelloWorldMessage : IHasRoutingKey
    {
        public string Message { get; set; }

        string IHasRoutingKey.RoutingKey
        {
            get { return "messages." + Message; }
        }
    }
}