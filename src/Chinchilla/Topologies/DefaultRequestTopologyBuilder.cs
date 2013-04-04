namespace Chinchilla.Topologies
{
    public class DefaultRequestTopologyBuilder : IMessageTopologyBuilder
    {
        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();
            topology.SubscribeQueue = topology.DefineQueue();
            return topology;
        }
    }
}
