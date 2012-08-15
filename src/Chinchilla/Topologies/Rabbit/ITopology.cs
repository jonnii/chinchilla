namespace Chinchilla.Topologies.Rabbit
{
    public interface ITopology
    {
        IQueue DefineQueue();

        IQueue DefineQueue(string name);

        IExchange DefineExchange(
            string name,
            ExchangeType exchangeType,
            Durability durablility = Durability.Durable,
            bool isAutoDelete = false);

        void Visit(ITopologyVisitor visitor);
    }
}