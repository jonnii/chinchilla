namespace Chinchilla
{
    public interface IMessageSerializers
    {
        IMessageSerializer Default { get; set; }

        IMessageSerializer Find(string contentType);

        IMessageSerializer FindOrDefault(string contentType);

        IMessageSerializer Register(IMessageSerializer serializer);
    }
}