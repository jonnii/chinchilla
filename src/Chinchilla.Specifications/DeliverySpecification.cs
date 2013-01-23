using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DeliverySpecification
    {
        [Subject(typeof(Delivery))]
        public class in_general : with_delivery
        {
            It should_be_replyable = () =>
                delivery.IsReplyable.ShouldBeTrue();
        }

        [Subject(typeof(Delivery))]
        public class without_correlation_id : with_delivery
        {
            Establish context = () =>
            {
                delivery = new Delivery(
                  listener,
                  1234,
                  new byte[] { 0xd, 0xe, 0xa, 0xd },
                  "routing-key",
                  "exchange",
                  "content-type",
                  "",
                  "reply-to");
            };

            It should_not_be_replyable = () =>
                delivery.IsReplyable.ShouldBeFalse();
        }

        [Subject(typeof(Delivery))]
        public class without_reply_to : with_delivery
        {
            Establish context = () =>
            {
                delivery = new Delivery(
                  listener,
                  1234,
                  new byte[] { 0xd, 0xe, 0xa, 0xd },
                  "routing-key",
                  "exchange",
                  "content-type",
                  "correlationId",
                  "");
            };

            It should_not_be_replyable = () =>
                delivery.IsReplyable.ShouldBeFalse();
        }

        [Subject(typeof(Delivery))]
        public class when_failing_delivery_with_exception : with_delivery
        {
            Because of = () =>
                delivery.Failed(new Exception());

            It should_notify_delivery_failure_strategy = () =>
                listener.WasToldTo(s => s.OnFailed(delivery, Param.IsAny<Exception>()));
        }

        public class with_delivery : WithFakes
        {
            Establish context = () =>
            {
                listener = An<IDeliveryListener>();

                delivery = new Delivery(
                    listener,
                    1234,
                    new byte[] { 0xd, 0xe, 0xa, 0xd },
                    "routing-key",
                    "exchange",
                    "content-type",
                    "correlationId",
                    "reply-to");
            };

            protected static Delivery delivery;

            protected static IDeliveryListener listener;
        }
    }
}
