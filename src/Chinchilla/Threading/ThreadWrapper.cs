using System.Threading;

namespace Chinchilla.Threading
{
    public class ThreadWrapper : IThread
    {
        private readonly Thread thread;

        public ThreadWrapper(Thread thread)
        {
            this.thread = thread;
        }

        public void Start()
        {
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }
    }
}