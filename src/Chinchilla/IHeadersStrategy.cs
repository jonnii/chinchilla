using System.Collections.Generic;

namespace Chinchilla
{
    public interface IHeadersStrategy<in TMessage>
    {
        Dictionary<object, object> PopulateHeaders(TMessage message);
    }
}
