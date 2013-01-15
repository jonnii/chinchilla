using System.Threading;

namespace Chinchilla.Threading
{
    public interface IThreadFactory
    {
        IThread Create(ThreadStart action);
    }
}
