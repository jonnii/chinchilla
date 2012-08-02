using System;

namespace Chinchilla
{
    public interface ISubscription : IDisposable
    {
        void Start();
    }
}