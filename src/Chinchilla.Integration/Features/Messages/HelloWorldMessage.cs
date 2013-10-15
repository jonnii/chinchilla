using System.Collections.Generic;

namespace Chinchilla.Integration.Features.Messages
{
    public class HelloWorldMessage : IHasRoutingKey, IHelloWorldMessage, IHasHeaders
    {
        public string Message { get; set; }

        string IHasRoutingKey.RoutingKey
        {
            get { return "messages." + Message; }
        }

        public void PopulateHeaders(IDictionary<object, object> headers)
        {
            headers.Add("key1", "foo");
            headers.Add("key2", "foo");
            headers.Add("key3", "foo");
        }
    }
}