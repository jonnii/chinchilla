using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class RequesterSpecification
    {
        [Subject(typeof(Requester<,>))]
        public class when_handling_response_with_no_correlation_id : with_requester
        {
            Because of = () =>
                exception = Catch.Exception(() => Subject.DispatchToRegisteredResponseHandler(new TestResponseMessage(), deliveryContext));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        [Subject(typeof(Requester<,>))]
        public class when_handling_response_with_unknown_correlation_id : with_requester
        {
            Establish context = () =>
                delivery.WhenToldTo(d => d.CorrelationId).Return("correlation-id");

            Because of = () =>
                exception = Catch.Exception(() => Subject.DispatchToRegisteredResponseHandler(new TestResponseMessage(), deliveryContext));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        [Subject(typeof(Requester<,>))]
        public class when_handling_response_with_registered_message : with_requester
        {
            Establish context = () =>
            {
                called = false;

                delivery.WhenToldTo(d => d.CorrelationId).Return("correlation-id");
                Subject.RegisterResponseHandler("correlation-id", _ => called = true);
            };

            Because of = () =>
                Subject.DispatchToRegisteredResponseHandler(new TestResponseMessage(), deliveryContext);

            It should_call_handler = () =>
                called.ShouldBeTrue();

            static bool called;
        }

        [Subject(typeof(Requester<,>))]
        public class when_register_response_handler_with_no_correlation_id : with_requester
        {
            Because of = () =>
                exception = Catch.Exception(() => Subject.RegisterResponseHandler("", _ => { }));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }

        public class with_requester : WithSubject<Requester<TestRequestMessage, TestResponseMessage>>
        {
            Establish context = () =>
            {
                delivery = An<IDelivery>();

                deliveryContext = An<IDeliveryContext>();
                deliveryContext.WhenToldTo(d => d.Delivery).Return(delivery);
            };

            protected static IDeliveryContext deliveryContext;

            protected static IDelivery delivery;
        }
    }
}
