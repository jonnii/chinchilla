namespace Chinchilla.Topologies.Rabbit
{
    public interface ITopology
    {
        IQueue DefineQueue();

        IQueue DefineQueue(string name);

        IExchange DefineExchange(string name, ExchangeType exchangeType);

        void Visit(ITopologyVisitor visitor);
    }
}