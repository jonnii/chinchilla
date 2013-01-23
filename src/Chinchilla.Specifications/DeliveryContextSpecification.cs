using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DeliveryContextSpecification
    {
        [Subject(typeof(DeliveryContext))]
        public class when_responding_to_delivery_without_correlation_id : WithSubject<DeliveryContext>
        {
            Establish context = () =>
                The<IDelivery>().WhenToldTo(d => d.IsReplyable).Return(false);

            Because of = () =>
                exception = Catch.Exception(() => Subject.Reply(new TestRequestMessage()));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<ChinchillaException>();

            static Exception exception;
        }
    }
}
