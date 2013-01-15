using System.Threading;

namespace Chinchilla.Threading
{
    public class ThreadFactory : IThreadFactory
    {
        public IThread Create(ThreadStart action)
        {
            return new ThreadWrapper(
                new Thread(action));
        }
    }
}