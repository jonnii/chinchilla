using System;

namespace Chinchilla
{
    public class DefaultConsumerFactory : IConsumerFactory
    {
        public IConsumer Build<T>()
            where T : IConsumer
        {
            return (IConsumer)Activator.CreateInstance(typeof(T));
        }

        public void Dispose()
        {

        }
    }
}