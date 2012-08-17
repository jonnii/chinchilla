using System;
using Chinchilla.Extensions;

namespace Chinchilla
{
    public abstract class Trackable : IDisposable
    {
        public event EventHandler<EventArgs> Disposed;

        public virtual void Dispose()
        {
            Disposed.Raise(this);
        }
    }
}