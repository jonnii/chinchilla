using System;
using System.Text;
using Chinchilla.Reflection;

namespace Chinchilla.Serializers
{
    public class JsonMessageSerializer : IMessageSerializer
    {
        private readonly IJsonSerializerStrategy strategy = new EnumSupportedStrategy();

        public string ContentType
        {
            get { return "application/json"; }
        }

        public byte[] Serialize<T>(IMessage<T> message)
        {
            var serialized = SimpleJson.SerializeObject(message, strategy);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public IMessage<T> Deserialize<T>(byte[] message)
        {
            var decoded = Encoding.UTF8.GetString(message);
            return SimpleJson.DeserializeObject<Message<T>>(decoded, strategy);
        }

        public class EnumSupportedStrategy : PocoJsonSerializerStrategy
        {
            protected override object SerializeEnum(Enum p)
            {
                return p.ToString();
            }

            public override object DeserializeObject(object value, Type type)
            {
                var stringValue = value as string;
                if (stringValue != null)
                {
                    if (type.IsEnum)
                    {
                        return Enum.Parse(type, stringValue);
                    }

                    if (ReflectionUtils.IsNullableType(type))
                    {
                        var underlyingType = Nullable.GetUnderlyingType(type);
                        if (underlyingType.IsEnum)
                        {
                            return Enum.Parse(underlyingType, stringValue);
                        }
                    }
                }

                return base.DeserializeObject(value, type);
            }
        }
    }
}
