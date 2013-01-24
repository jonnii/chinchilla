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

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_confirming_single_receipt : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(new ConfirmReceipt(200));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Confirmed());

            It should_be_confirmed = () =>
                receipt.IsConfirmed.ShouldBeTrue();

            private static ConfirmReceipt receipt;
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_confirming_single_failed_receipt : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(new ConfirmReceipt(200));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Failed());

            It should_be_confirmed = () =>
                receipt.IsFailed.ShouldBeTrue();

            private static ConfirmReceipt receipt;
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_confirming_multiple_receipts : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
            {
                first = Subject.RegisterReceipt(new ConfirmReceipt(200));
                second = Subject.RegisterReceipt(new ConfirmReceipt(201));
                third = Subject.RegisterReceipt(new ConfirmReceipt(202));
                fourth = Subject.RegisterReceipt(new ConfirmReceipt(203));
            };

            Because of = () =>
                Subject.ProcessReceipts(true, 202, r => r.Confirmed());

            It should_confirm_all_receipts = () =>
            {
                first.IsConfirmed.ShouldBeTrue();
                second.IsConfirmed.ShouldBeTrue();
                third.IsConfirmed.ShouldBeTrue();
            };

            It should_not_confirm_unaffected_receipts = () =>
                fourth.IsConfirmed.ShouldBeFalse();

            private static ConfirmReceipt first;

            private static ConfirmReceipt second;

            private static ConfirmReceipt third;

            private static ConfirmReceipt fourth;
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
        public class when_registering_duplicate_receipt : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () =>
                Subject.RegisterReceipt(new ConfirmReceipt(300));

            Because of = () =>
                exception = Catch.Exception(() => Subject.RegisterReceipt(new ConfirmReceipt(300)));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<DuplicatePublishReceiptException>();

            static Exception exception;
        }

        [Subject(typeof(ConfirmingPublisher<>))]
        public class when_registering_receipt : WithSubject<ConfirmingPublisher<TestMessage>>
        {
            Establish context = () => { };

            Because of = () =>
                Subject.RegisterReceipt(new ConfirmReceipt(300));

            It should_have_receipt_registered_for_receipt = () =>
                Subject.HasPendingConfirmation(300).ShouldBeTrue();
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
