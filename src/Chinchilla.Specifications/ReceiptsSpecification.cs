using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ReceiptsSpecification
    {
        [Subject(typeof(Receipts<>))]
        public class when_confirming_single_receipt : WithSubject<Receipts<TestMessage>>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(new ConfirmReceipt<TestMessage>(200, new TestMessage()));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Confirmed());

            It should_be_confirmed = () =>
                receipt.IsConfirmed.ShouldBeTrue();

            static ConfirmReceipt<TestMessage> receipt;
        }

        [Subject(typeof(Receipts<>))]
        public class when_confirming_single_failed_receipt : WithSubject<Receipts<TestMessage>>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(ConfirmReceipt.New(200, new TestMessage()));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Failed(PublishFailureReason.Nack));

            It should_be_confirmed = () =>
                receipt.IsFailed.ShouldBeTrue();

            static ConfirmReceipt<TestMessage> receipt;
        }

        [Subject(typeof(Receipts<>))]
        public class when_registering_duplicate_receipt : WithSubject<Receipts<TestMessage>>
        {
            Establish context = () =>
                Subject.RegisterReceipt(ConfirmReceipt.New(300, new TestMessage()));

            Because of = () =>
                exception = Catch.Exception(() => Subject.RegisterReceipt(ConfirmReceipt.New(300, new TestMessage())));

            It should_throw_exception = () =>
                exception.ShouldBeAssignableTo<DuplicatePublishReceiptException>();

            static Exception exception;
        }

        [Subject(typeof(Receipts<>))]
        public class when_confirming_multiple_receipts : WithSubject<Receipts<TestMessage>>
        {
            Establish context = () =>
            {
                first = Subject.RegisterReceipt(ConfirmReceipt.New(200, new TestMessage()));
                second = Subject.RegisterReceipt(ConfirmReceipt.New(201, new TestMessage()));
                third = Subject.RegisterReceipt(ConfirmReceipt.New(202, new TestMessage()));
                fourth = Subject.RegisterReceipt(ConfirmReceipt.New(203, new TestMessage()));
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

            static ConfirmReceipt<TestMessage> first;

            static ConfirmReceipt<TestMessage> second;

            static ConfirmReceipt<TestMessage> third;

            static ConfirmReceipt<TestMessage> fourth;
        }

        [Subject(typeof(Receipts<>))]
        public class when_confirming_all_receipts : WithSubject<Receipts<TestMessage>>
        {
            Establish context = () =>
            {
                receipts = new List<ConfirmReceipt<TestMessage>>()
                {
                    ConfirmReceipt.New(200, new TestMessage()),
                    ConfirmReceipt.New(201, new TestMessage()),
                    ConfirmReceipt.New(202, new TestMessage())
                };

                receipts.ForEach(r => Subject.RegisterReceipt(r));
            };

            Because of = () =>
                Subject.ProcessAllReceipts(r => r.Failed(PublishFailureReason.Disconnected));

            It should_fail_all_receipts_with_specified_reason = () =>
            {
                receipts.ShouldEachConformTo(r => r.IsFailed);
                receipts.ShouldEachConformTo(r => r.FailureReason == PublishFailureReason.Disconnected);
            };

            static List<ConfirmReceipt<TestMessage>> receipts;
        }

        [Subject(typeof(Receipts<>))]
        public class when_registering_receipt : WithSubject<Receipts<TestMessage>>
        {
            Because of = () =>
                Subject.RegisterReceipt(ConfirmReceipt.New(300, new TestMessage()));

            It should_have_receipt_registered_for_receipt = () =>
                Subject.HasPendingConfirmation(300).ShouldBeTrue();
        }

        public class TestMessage { }
    }
}
