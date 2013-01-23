namespace Chinchilla.Topologies
{
    public class DefaultResponseTopology : IMessageTopologyBuilder
    {
        private readonly string replyToQueueName;

        public DefaultResponseTopology(string replyToQueueName)
        {
            this.replyToQueueName = replyToQueueName;
        }

        public IMessageTopology Build(IEndpoint endpoint)
        {
            var topology = new MessageTopology();
            topology.PublishTarget = topology.DefineQueue(replyToQueueName);
            return topology;
        }
    }
}
