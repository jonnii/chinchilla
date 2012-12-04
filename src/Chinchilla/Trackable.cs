using System;
using Chinchilla.Extensions;

namespace Chinchilla
{
    public abstract class Trackable : IDisposable
    {
        private bool isDisposed;

        public event EventHandler<EventArgs> Disposed;

        public virtual void Dispose()
        {
            if (isDisposed)
            {
                var message = string.Format("Object already disposed: {0}", this);
                throw new ObjectDisposedException(message);
            }

            isDisposed = true;

            Disposed.Raise(this);
        }
    }
}