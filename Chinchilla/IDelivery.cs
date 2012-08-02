namespace Chinchilla
{
    public interface IDelivery
    {
        ulong Tag { get; }

        byte[] Body { get; }

        void Accept();
    }
}