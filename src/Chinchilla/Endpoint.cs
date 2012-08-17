namespace Chinchilla
{
    public class Endpoint : IEndpoint
    {
        public Endpoint(
            string endpointName,
            string messageType)
        {
            Name = endpointName;
            MessageType = messageType;
        }

        public string Name { get; private set; }

        public string MessageType { get; private set; }
    }
}
