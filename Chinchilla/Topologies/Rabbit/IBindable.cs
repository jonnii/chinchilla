namespace Chinchilla.Topologies.Rabbit
{
    public interface IBindable
    {
        void BindTo(IExchange exchange, params string[] routingKeys);

        void Visit(ITopologyVisitor visitor);
    }
}
