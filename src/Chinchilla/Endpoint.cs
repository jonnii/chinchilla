namespace Chinchilla
{
    public class Endpoint
    {
        public Endpoint(
            string endpointName,
            string messageType)
        {
            EndpointName = endpointName;
            MessageType = messageType;
        }

        public string EndpointName { get; private set; }

        public string MessageType { get; private set; }
    }
}
