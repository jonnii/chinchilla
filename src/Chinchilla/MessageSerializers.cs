using System;
using System.Collections.Generic;
using Chinchilla.Serializers;

namespace Chinchilla
{
    public class MessageSerializers : IMessageSerializers
    {
        private IMessageSerializer defaultSerializer;

        private readonly Dictionary<string, IMessageSerializer> serializersByContentType =
            new Dictionary<string, IMessageSerializer>();

        public MessageSerializers()
        {
            Default = Register(new JsonMessageSerializer());
        }

        public IMessageSerializer Default
        {
            get { return defaultSerializer; }
            set
            {
                defaultSerializer = Register(value);
            }
        }

        public IMessageSerializer FindOrDefault(string contentType)
        {
            return string.IsNullOrEmpty(contentType)
                  ? Default
                  : Find(contentType);
        }

        public IMessageSerializer Register(IMessageSerializer serializer)
        {
            var contentType = serializer.ContentType;

            if (!serializersByContentType.ContainsKey(contentType))
            {
                serializersByContentType.Add(contentType, serializer);
            }

            return serializer;
        }

        public IMessageSerializer Find(string contentType)
        {
            Guard.NotNull(contentType, "contentType");

            IMessageSerializer serializer;
            if (!serializersByContentType.TryGetValue(contentType, out serializer))
            {
                var message = string.Format(
                    "There is no serializer registered with the content type '{0}', " +
                    "you need to register it with the DepotSettings when creating the bus.",
                    contentType);

                throw new NotSupportedException(message);
            }

            return serializer;
        }
    }
}