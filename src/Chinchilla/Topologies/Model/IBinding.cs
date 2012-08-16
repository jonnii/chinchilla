namespace Chinchilla.Topologies.Model
{
    public interface IBinding
    {
        IBindable Bindable { get; }

        IExchange Exchange { get; }

        string[] RoutingKeys { get; }

        void Visit(ITopologyVisitor visitor);
    }
}