using System.Collections.Generic;

namespace Chinchilla.Topologies.Model
{
    public interface IBindable
    {
        string Name { get; set; }

        bool HasName { get; }

        IEnumerable<IBinding> Bindings { get; }

        void BindTo(IExchange exchange, params string[] routingKeys);

        void Visit(ITopologyVisitor visitor);
    }
}
