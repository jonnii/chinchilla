namespace Chinchilla.Topologies.Rabbit
{
    public interface IBinding
    {
        void Visit(ITopologyVisitor visitor);
    }
}