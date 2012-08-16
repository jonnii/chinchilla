namespace Chinchilla.Integration.Features.Messages
{
    public class HelloWorldMessage : IHasRoutingKey
    {
        public string Message { get; set; }

        public string RoutingKey
        {
            get { return "messages." + Message; }
        }
    }
}