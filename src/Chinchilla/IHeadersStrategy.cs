using System.Collections.Generic;

namespace Chinchilla
{
    public interface IHeadersStrategy<in TMessage>
    {
        void PopulateHeaders(TMessage message, IDictionary<object, object> headers);
    }
}
