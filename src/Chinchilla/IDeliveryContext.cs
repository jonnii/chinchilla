namespace Chinchilla
{
    /// <summary>
    /// A delivery context gives you access to the context of a delivery
    /// </summary>
    public interface IDeliveryContext
    {
        IBus Bus { get; }
    }
}