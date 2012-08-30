using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ErrorQueueDeliveryFailureStrategySpecification
    {
        [Subject(typeof(ErrorQueueDeliveryFailureStrategy))]
        public class when_handling_failed_delivery : with_strategy
        {
            Establish context = () =>
                delivery = An<IDelivery>();

            Because of = () =>
                Subject.Handle(delivery, new Exception());

            It should_accept_delivery = () =>
                delivery.WasToldTo(d => d.Accept());

            static IDelivery delivery;
        }

        public class with_strategy : WithFakes
        {
            Establish context = () =>
            {
                var publisher = An<IPublisher<Error>>();

                var bus = An<IBus>();
                bus.WhenToldTo(b => b.CreatePublisher<Error>(
                    Param.IsAny<Action<IPublisherBuilder>>())).Return(publisher);

                Subject = new ErrorQueueDeliveryFailureStrategy(bus);
            };

            protected static ErrorQueueDeliveryFailureStrategy Subject;

            protected static IPublisher<Error> publisher;
        }
    }
}
