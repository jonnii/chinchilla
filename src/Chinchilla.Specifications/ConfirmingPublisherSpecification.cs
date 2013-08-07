using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;

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
        public class when_publishing_with_receipt : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
            {
                model = An<IModel>();
                model.WhenToldTo(m => m.NextPublishSeqNo).Return(300);
            };

            Because of = () =>
                receipt = (ConfirmReceipt)Subject.PublishWithReceipt(model, "key", An<IBasicProperties>(), new byte[0]);

            It should_set_sequence_number_on_confirm_receipt = () =>
                receipt.Sequence.ShouldEqual((ulong)300);

            It should_have_registered_receipt = () =>
                Subject.HasPendingConfirmation(300).ShouldBeTrue();

            static IModel model;

            static ConfirmReceipt receipt;
        }

        public class with_model_reference : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
            {
                model = An<IModel>();

                The<IModelReference>().WhenToldTo(r => r.Execute(Param.IsAny<Action<IModel>>()))
                    .Callback<Action<IModel>>(act => act(model));
            };

            protected static IModel model;
        }
    }
}
