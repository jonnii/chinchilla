namespace Chinchilla
{
    /// <summary>
    /// A bus concern allows for start up features that are orthogonal
    /// to the the buses core features
    /// </summary>
    public interface IBusConcern
    {
        /// <summary>
        /// Runs the bus concern
        /// </summary>
        void Run(IBus bus);
    }
}