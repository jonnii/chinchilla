namespace Chinchilla.Topologies.Model
{
    public interface ITopologyVisitor
    {
        void Visit(IQueue queue);

        void Visit(IExchange exchange);

        void Visit(IBinding binding);
    }
}