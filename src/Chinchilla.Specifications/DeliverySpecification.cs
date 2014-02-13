using System;
using System.Collections.Generic;
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
        public class with_multiple_listeners : with_delivery
        {
            Establish context = () =>
            {
                anotherListener = An<IDeliveryListener>();
                delivery.RegisterDeliveryListener(anotherListener);
            };

            Because of = () =>
                delivery.Accept();

            It should_notify_all_listeners = () =>
            {
                listener.WasToldTo(l => l.OnAccept(Param.IsAny<IDelivery>()));
                anotherListener.WasToldTo(l => l.OnAccept(Param.IsAny<IDelivery>()));
            };

            It should_clear_registered_deliveries_after_accept = () =>
                delivery.HasRegisteredDeliveryListeners.ShouldBeFalse();

            static IDeliveryListener anotherListener;
        }

        [Subject(typeof(Delivery))]
        public class without_correlation_id : with_delivery
        {
            Establish context = () =>
            {
                delivery = new Delivery(
                  1234,
                  new byte[] { 0xd, 0xe, 0xa, 0xd },
                  "routing-key",
                  "exchange",
                  "content-type",
                  "",
                  "reply-to",
                  new Dictionary<string, object>());
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
                  1234,
                  new byte[] { 0xd, 0xe, 0xa, 0xd },
                  "routing-key",
                  "exchange",
                  "content-type",
                  "correlationId",
                  "",
                  new Dictionary<string, object>());
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

            It should_clear_registered_deliveries_after_failed = () =>
                delivery.HasRegisteredDeliveryListeners.ShouldBeFalse();
        }

        [Subject(typeof(Delivery))]
        public class when_rejecting_delivery_with_requeue : with_delivery
        {
            Because of = () =>
                delivery.Reject(true);

            It should_notify_delivery_failure_strategy = () =>
                listener.WasToldTo(s => s.OnReject(delivery, Param.Is(true)));

            It should_clear_registered_deliveries_after_failed = () =>
                delivery.HasRegisteredDeliveryListeners.ShouldBeFalse();
        }

        public class with_delivery : WithFakes
        {
            Establish context = () =>
            {
                listener = An<IDeliveryListener>();

                delivery = new Delivery(
                    1234,
                    new byte[] { 0xd, 0xe, 0xa, 0xd },
                    "routing-key",
                    "exchange",
                    "content-type",
                    "correlationId",
                    "reply-to",
                    new Dictionary<string, object>());

                delivery.RegisterDeliveryListener(listener);
            };

            protected static Delivery delivery;

            protected static IDeliveryListener listener;
        }
    }
}
