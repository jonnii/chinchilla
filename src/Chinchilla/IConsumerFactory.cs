using System;

namespace Chinchilla
{
    public interface IConsumerFactory : IDisposable
    {
        IConsumer Build<T>()
            where T : IConsumer;
    }
}
