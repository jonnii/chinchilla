namespace Chinchilla.Topologies
{
    public interface IMessageTopologyBuilder
    {
        IMessageTopology Build(IEndpoint endpoint);
    }
}