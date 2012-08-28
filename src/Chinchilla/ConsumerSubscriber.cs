using System;
using System.Linq;
using System.Reflection;

namespace Chinchilla
{
    public class ConsumerSubscriber
    {
        private static readonly MethodInfo subscribeMethod;

        private static readonly MethodInfo subscribeMethodWithBuilder;

        static ConsumerSubscriber()
        {
            subscribeMethod = typeof(IBus).GetMethods()
                .Single(m => m.Name == "Subscribe"
                && m.GetGenericArguments().Count() == 1
                && m.GetParameters().Count() == 1);

            subscribeMethodWithBuilder = typeof(IBus).GetMethods()
                .Single(m => m.Name == "Subscribe"
                && m.GetGenericArguments().Count() == 1
                && m.GetParameters().Count() == 2);
        }

        private readonly IBus bus;

        private readonly IConsumer consumer;

        public ConsumerSubscriber(IBus bus, IConsumer consumer)
        {
            this.bus = bus;
            this.consumer = consumer;
        }

        public ISubscription Connect()
        {
            return Connect(subscribeMethod, new object[1]);
        }

        public ISubscription Connect(Action<ISubscriptionBuilder> builder)
        {
            return Connect(subscribeMethodWithBuilder, new object[] { null, builder });
        }

        private ISubscription Connect(MethodInfo methodInfo, object[] parameters)
        {
            var method = consumer.GetType().GetMethod("Consume");
            var messageType = method.GetParameters().First().ParameterType;

            var actionType = typeof(Action<>).MakeGenericType(messageType);
            var consumeAction = Delegate.CreateDelegate(actionType, consumer, method);

            var genericSubscribeMethod = methodInfo.MakeGenericMethod(messageType);

            parameters[0] = consumeAction;

            return (ISubscription)genericSubscribeMethod.Invoke(bus, parameters);
        }
    }
}