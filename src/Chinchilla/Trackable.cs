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
                throw new ObjectDisposedException($"Object already disposed: {this}");
            }

            isDisposed = true;

            Disposed.Raise(this);
        }
    }
}