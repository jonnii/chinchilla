namespace Chinchilla.Topologies.Rabbit
{
    public interface ITopology
    {
        IQueue DefineQueue();

        IExchange DefineExchange(string name, ExchangeType exchangeType);

        void Visit(ITopologyVisitor visitor);
    }
}