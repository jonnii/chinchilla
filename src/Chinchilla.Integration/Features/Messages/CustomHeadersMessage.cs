using System.Collections.Generic;

namespace Chinchilla.Integration.Features.Messages
{
    public class CustomHeadersMessage : IHasHeaders
    {
        public void PopulateHeaders(IDictionary<object, object> headers)
        {
            headers.Add("key1", "foo");
            headers.Add("key2", "foo");
            headers.Add("key3", "foo");
        }
    }
}
