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
                serializer = An<IMessageSerializer>();
                serializer.WhenToldTo(s => s.Deserialize<TestMessage>(Param.IsAny<byte[]>()))
                    .Return(new Message<TestMessage>(new TestMessage()));

                Subject = new ActionDeliveryProcessor<TestMessage>(serializer, (m, c) =>
                {
                    messageContext = c;
                });

                delivery = An<IDelivery>();
            };

            Because of = () =>
                Subject.Process(delivery);

            It should_pass_message_context_to_callback = () =>
                messageContext.ShouldNotBeNull();

            static ActionDeliveryProcessor<TestMessage> Subject;

            static IMessageSerializer serializer;

            static IMessageContext messageContext;

            static IDelivery delivery;
        }

        public class TestMessage { }
    }
}
