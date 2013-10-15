using System.Collections.Generic;

namespace Chinchilla
{
    public interface IHasHeaders
    {
        void PopulateHeaders(IDictionary<object, object> headers);
    }
}
