using System;

namespace Chinchilla
{
    public class Depot : IDisposable
    {
        public static IBus Connect(string server)
        {
            return new Bus();
        }

        public void Dispose()
        {

        }
    }
}
