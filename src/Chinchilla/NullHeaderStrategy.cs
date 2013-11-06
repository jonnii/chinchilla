using System.Collections.Generic;

namespace Chinchilla
{
    public class NullHeaderStrategy<TMessage> : IHeadersStrategy<TMessage>
    {
        public Dictionary<object, object> PopulateHeaders(TMessage message)
        {
            return null;
        }
    }
}