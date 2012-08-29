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

                Subject = new ActionDeliveryProcessor<TestMessage>(bus, serializer, (m, c) =>
                {
                    deliveryContext = c;
                });

                delivery = An<IDelivery>();
            };

            Because of = () =>
                Subject.Process(delivery);

            It should_pass_message_context_to_callback = () =>
                deliveryContext.ShouldNotBeNull();

            static ActionDeliveryProcessor<TestMessage> Subject;

            static IMessageSerializer serializer;

            static IDeliveryContext deliveryContext;

            static IDelivery delivery;

            static IBus bus;
        }

        public class TestMessage { }
    }
}
