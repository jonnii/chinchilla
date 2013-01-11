using System;
using System.Linq;
using System.Reflection;
using Chinchilla.Configuration;

namespace Chinchilla
{
    public class ConsumerSubscriber
    {
        private static readonly MethodInfo subscribeMethod;

        private static readonly MethodInfo subscribeMethodWithConfiguration;

        static ConsumerSubscriber()
        {
            subscribeMethod = typeof(IBus).GetMethods()
                .Single(m =>
                    m.Name == "Subscribe" &&
                    m.GetGenericArguments().Count() == 1 &&
                    m.GetParameters().Count() == 1 &&
                    m.GetParameters().First().ParameterType.GetGenericArguments().Count() == 2);

            subscribeMethodWithConfiguration = typeof(IBus).GetMethods()
                .Single(m =>
                    m.Name == "Subscribe" &&
                    m.GetGenericArguments().Count() == 1 &&
                    m.GetParameters().Count() == 2 &&
                    m.GetParameters().First().ParameterType.GetGenericArguments().Count() == 2);
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
            var interfaces = consumer
                .GetType()
                .GetInterfaces()
                .Where(i => typeof(IConsumer).IsAssignableFrom(i) && i != typeof(IConsumer));

            if (!interfaces.Any())
            {
                throw new ChinchillaException(
                    "Could not find any interfaces that are assignable to IConsumer, did you implement IConsumer<T>?");
            }

            if (interfaces.Count() > 1)
            {
                throw new ChinchillaException(
                    "An consumer cannot consume more than one type of message.");
            }

            var consumerType = interfaces.Single();

            return ConnectConsumerInterface(consumerType);
        }

        private ISubscription ConnectConsumerInterface(Type consumerInterfaceType)
        {
            var method = consumerInterfaceType.GetMethod("Consume");

            if (method == null)
            {
                throw new ChinchillaException(
                    "Could not get Consume method from consumer, did you try to subscribe to IConsumer, instead of IConsumer<T>?");
            }

            var messageType = method.GetParameters()
                .First()
                .ParameterType;

            var actionType = typeof(Action<,>).MakeGenericType(messageType, typeof(IDeliveryContext));
            var consumeAction = Delegate.CreateDelegate(actionType, consumer, method);

            if (consumer is IConfigurableConsumer)
            {
                var configureMethod = consumer
                    .GetType()
                    .GetMethod("ConfigureSubscription");

                var configureAction = Delegate.CreateDelegate(
                    typeof(Action<ISubscriptionBuilder>),
                    consumer,
                    configureMethod);

                var genericSubscribeMethod = subscribeMethodWithConfiguration.MakeGenericMethod(messageType);

                return (ISubscription)genericSubscribeMethod.Invoke(bus, new object[] { consumeAction, configureAction });
            }
            else
            {
                var genericSubscribeMethod = subscribeMethod.MakeGenericMethod(messageType);

                return (ISubscription)genericSubscribeMethod.Invoke(bus, new object[] { consumeAction });
            }
        }
    }
}