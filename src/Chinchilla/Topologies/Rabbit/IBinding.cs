namespace Chinchilla.Topologies.Rabbit
{
    public interface IBinding
    {
        IBindable Bindable { get; }

        IExchange Exchange { get; }

        void Visit(ITopologyVisitor visitor);
    }
}