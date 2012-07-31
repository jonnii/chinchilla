namespace Chinchilla.Topologies.Rabbit
{
    public class Binding : IBinding
    {
        public Binding(IBindable bindable, Exchange exchange)
            : this(bindable, exchange, new string[0])
        {

        }

        public Binding(IBindable bindable, IExchange exchange, string[] routingKeys)
        {
            Bindable = bindable;
            Exchange = exchange;
            RoutingKeys = routingKeys;
        }

        public IBindable Bindable { get; private set; }

        public IExchange Exchange { get; private set; }

        public string[] RoutingKeys { get; private set; }

        public void Visit(ITopologyVisitor visitor)
        {
            Exchange.Visit(visitor);
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return string.Format("[Binding Bindable: {0}, Exchange: {1}]", Bindable, Exchange);
        }
    }
}