using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ActionDeliveryProcessorSpecification
    {
        [Subject(typeof(ActionDeliveryProcessor<>))]
        public class when_processing_delivery : WithFakes
        {
            Establish context = () =>
            {
                bus = An<IBus>();

                serializer = An<IMessageSerializer>();
                serializer.WhenToldTo(s => s.Deserialize<TestMessage>(Param.IsAny<byte[]>()))
                    .Return(new Message<TestMessage>(new TestMessage()));

                serializers = An<IMessageSerializers>();
                serializers.WhenToldTo(s => s.FindOrDefault("content-type"))
                    .Return(serializer);

                Subject = new ActionDeliveryProcessor<TestMessage>(bus, serializers, (m, c) =>
                {
                    deliveryContext = c;
                });

                delivery = An<IDelivery>();
                delivery.WhenToldTo(d => d.ContentType).Return("content-type");
            };

            Because of = () =>
                Subject.Process(delivery);

            It should_pass_message_context_to_callback = () =>
                deliveryContext.ShouldNotBeNull();

            static ActionDeliveryProcessor<TestMessage> Subject;

            static IMessageSerializers serializers;

            static IMessageSerializer serializer;

            static IDeliveryContext deliveryContext;

            static IDelivery delivery;

            static IBus bus;
        }

        public class TestMessage { }
    }
}
