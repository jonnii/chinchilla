namespace Chinchilla.Topologies.Rabbit
{
    public interface IQueue : IBindable
    {
        string Name { get; }
    }
}