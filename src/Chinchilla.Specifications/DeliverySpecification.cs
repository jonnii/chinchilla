﻿using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class DeliverySpecification
    {
        [Subject(typeof(Delivery))]
        public class when_failing_delivery_with_exception : WithFakes
        {
            Establish context = () =>
            {
                listener = An<IDeliveryListener>();

                delivery = new Delivery(
                    listener, 1234, new byte[] { 0xd, 0xe, 0xa, 0xd }, "routing-key", "exchange", "content-type");
            };

            Because of = () =>
                delivery.Failed(new Exception());

            It should_notify_delivery_failure_strategy = () =>
                listener.WasToldTo(s => s.OnFailed(delivery, Param.IsAny<Exception>()));

            static Delivery delivery;

            static IDeliveryListener listener;
        }
    }
}
