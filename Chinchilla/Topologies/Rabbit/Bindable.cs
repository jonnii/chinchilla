using System.Collections.Generic;
using System.Linq;

namespace Chinchilla.Topologies.Rabbit
{
    public abstract class Bindable : IBindable
    {
        private readonly List<IBinding> bindings = new List<IBinding>();

        public string Name { get; set; }

        public bool HasName
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        public IEnumerable<IBinding> Bindings
        {
            get { return bindings; }
        }

        public bool HasBindings
        {
            get { return bindings.Any(); }
        }

        public void BindTo(IExchange exchange, params string[] routingKeys)
        {
            bindings.Add(new Binding(this, exchange, routingKeys));
        }

        public abstract void Visit(ITopologyVisitor visitor);
    }
}
