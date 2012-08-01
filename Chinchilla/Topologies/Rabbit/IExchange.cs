namespace Chinchilla.Topologies.Rabbit
{
    public interface IExchange : IBindable
    {
        ExchangeType Type { get; }
    }
}