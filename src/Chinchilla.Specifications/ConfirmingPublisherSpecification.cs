using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Chinchilla.Specifications
{
    public class ConfirmingPublisherSpecification
    {
        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_starting : with_model_reference
        {
            Because of = () =>
                Subject.Start();

            It should_confirm_select = () =>
                model.WasToldTo(m => m.ConfirmSelect());
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_disposing : with_model_reference
        {
            Because of = () =>
                Subject.Dispose();

            It should_confirm_select = () =>
                model.WasToldTo(m => m.WaitForConfirmsOrDie());
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_publishing_with_receipt : with_model_reference
        {
            Because of = () =>
                receipt = (ConfirmReceipt<TestMessage>)Subject.PublishWithReceipt(
                    new TestMessage(),
                    model,
                    "key",
                    An<IBasicProperties>(),
                    new byte[0]);

            It should_set_sequence_number_on_confirm_receipt = () =>
                receipt.Sequence.ShouldEqual((ulong)300);

            It should_have_registered_receipt = () =>
                Subject.HasPendingConfirmation(300).ShouldBeTrue();

            static ConfirmReceipt<TestMessage> receipt;
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_message_is_nacked : with_model_reference
        {
            Establish context = () =>
            {
                publishFaultAction = An<IPublishFaultAction<TestMessage>>();
                originalMessage = new TestMessage();

                Subject.PublishWithReceipt(originalMessage, model, "routingkey", An<IBasicProperties>(), new byte[0]);

                The<IPublishFaultStrategy>().WhenToldTo(s => s.OnFailedReceipt<TestMessage>(Param.IsAny<IPublishReceipt>()))
                    .Return(publishFaultAction);
            };

            Because of = () =>
                Subject.OnBasicNacks(model, new BasicNackEventArgs { DeliveryTag = 300, Multiple = false, Requeue = false });

            It should_ask_strategy_publish_fault = () =>
                publishFaultAction.WasToldTo(s => s.Run(Param.IsAny<IPublisher<TestMessage>>(), originalMessage));

            static IPublishFaultAction<TestMessage> publishFaultAction;

            static TestMessage originalMessage;
        }

        public class with_model_reference : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
            {
                model = An<IModel>();
                model.WhenToldTo(m => m.NextPublishSeqNo).Return(300);

                The<IModelReference>().WhenToldTo(r => r.Execute(Param.IsAny<Action<IModel>>()))
                    .Callback<Action<IModel>>(act => act(model));
            };

            protected static IModel model;
        }
    }
}
