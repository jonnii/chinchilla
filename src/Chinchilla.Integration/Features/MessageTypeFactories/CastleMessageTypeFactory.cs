using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Chinchilla.Serializers;

namespace Chinchilla.Integration.Features.MessageTypeFactories
{
    /// <summary>
    /// This is an example castle message type factory, do not use this in production code!
    /// </summary>
    public class CastleMessageTypeFactory : IMessageTypeFactory
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public Func<object> GetTypeFactory(Type key)
        {
            return () => generator.CreateInterfaceProxyWithoutTarget(key, new BasicInterceptor());
        }

        public class BasicInterceptor : IInterceptor
        {
            private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

            public void Intercept(IInvocation invocation)
            {
                var methodName = invocation.Method.Name;
                if (methodName.StartsWith("set_"))
                {
                    var propertyName = methodName.Substring(4);
                    properties[propertyName] = invocation.Arguments[0];
                    return;
                }

                if (methodName.StartsWith("get_"))
                {
                    var propertyName = methodName.Substring(4);
                    var value = properties[propertyName];
                    invocation.ReturnValue = value;
                    return;
                }

                throw new ArgumentException("Only works with properties");
            }
        }
    }
}
