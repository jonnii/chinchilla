using System.Collections.Generic;

namespace Chinchilla.Specifications.Messages
{
    public class CustomHeadersMessage : IHasHeaders
    {
        public void PopulateHeaders(IDictionary<object, object> headers)
        {
            headers.Add("foo1", "bar");
            headers.Add("foo2", "bar");
            headers.Add("foo3", "bar");
            headers.Add("foo4", "bar");
            headers.Add("foo5", "bar");
        }
    }
}
