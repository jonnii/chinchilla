namespace Chinchilla
{
    public interface IModelFactory
    {
        bool IsOpen { get; }

        IModelReference CreateModel();
    }
}