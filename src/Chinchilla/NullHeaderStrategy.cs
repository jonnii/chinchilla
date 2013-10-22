using System.Collections.Generic;

namespace Chinchilla
{
    public class NullHeaderStrategy<TMessage> : IHeadersStrategy<TMessage>
    {
        public void PopulateHeaders(TMessage message, IDictionary<object, object> headers)
        {
            
        }
    }
}