namespace Chinchilla.Topologies.Model
{
    public interface IExchange : IBindable
    {
        ExchangeType Type { get; }

        Durability Durability { get; }

        bool IsAutoDelete { get; }

        bool IsInternal { get; }

        string AlternateExchange { get; }
    }
}