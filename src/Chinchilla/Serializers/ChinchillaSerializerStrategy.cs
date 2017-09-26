using System;
using System.Reflection;
using Chinchilla.Reflection;

namespace Chinchilla.Serializers
{
    public class ChinchillaSerializerStrategy : PocoJsonSerializerStrategy
    {
        private readonly IMessageTypeFactory messageTypeFactory;

        public ChinchillaSerializerStrategy(IMessageTypeFactory messageTypeFactory)
        {
            this.messageTypeFactory = messageTypeFactory;
        }

        protected override object SerializeEnum(Enum p)
        {
            return p.ToString();
        }

        public override object DeserializeObject(object value, Type type)
        {
            var stringValue = value as string;

            if (stringValue != null)
            {
                if (type.GetTypeInfo().IsEnum)
                {
                    return Enum.Parse(type, stringValue);
                }

                if (ReflectionUtils.IsNullableType(type))
                {
                    var underlyingType = Nullable.GetUnderlyingType(type);
                    if (underlyingType.GetTypeInfo().IsEnum)
                    {
                        return Enum.Parse(underlyingType, stringValue);
                    }
                }
            }

            return base.DeserializeObject(value, type);
        }

        internal override ReflectionUtils.ConstructorDelegate ContructorDelegateFactory(Type key)
        {
            if (!key.GetTypeInfo().IsInterface)
            {
                return base.ContructorDelegateFactory(key);
            }

            var factory = messageTypeFactory.GetTypeFactory(key);

            ReflectionUtils.ConstructorDelegate constructorDelegate = delegate
            {
                return factory();
            };

            return constructorDelegate;
        }
    }
}