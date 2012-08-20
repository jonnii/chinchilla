namespace Chinchilla
{
    public interface IRouter
    {
        string Route<TMessage>(TMessage message);
    }
}