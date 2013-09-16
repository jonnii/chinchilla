using System;

namespace Chinchilla.Serializers
{
    /// <summary>
    /// A message type factory is responsible for creating a function that can return the implementation
    /// of an interface through a function builder.
    /// </summary>
    public interface IMessageTypeFactory
    {
        Func<object> GetTypeFactory(Type key);
    }
}