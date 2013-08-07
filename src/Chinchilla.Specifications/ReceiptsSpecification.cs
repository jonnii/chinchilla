using System;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class ReceiptsSpecification
    {
        [Subject(typeof(Receipts))]
        public class when_confirming_single_receipt : WithSubject<Receipts>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(new ConfirmReceipt(200));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Confirmed());

            It should_be_confirmed = () =>
                receipt.IsConfirmed.ShouldBeTrue();

            private static ConfirmReceipt receipt;
        }

        [Subject(typeof(Receipts))]
        public class when_confirming_single_failed_receipt : WithSubject<Receipts>
        {
            Establish context = () =>
                 receipt = Subject.RegisterReceipt(new ConfirmReceipt(200));

            Because of = () =>
                Subject.ProcessReceipts(false, 200, r => r.Failed());

            It should_be_confirmed = () =>
                receipt.IsFailed.ShouldBeTrue();

            private static ConfirmReceipt receipt;
        }

        [Subject(typeof(Receipts))]
        public class when_registering_duplicate_receipt : WithSubject<Receipts>
        {
            Establish context = () =>
                Subject.RegisterReceipt(new ConfirmReceipt(300));

            Because of = () =>
                exception = Catch.Exception(() => Subject.RegisterReceipt(new ConfirmReceipt(300)));

            It should_throw_exception = () =>
                exception.ShouldBeOfType<DuplicatePublishReceiptException>();

            static Exception exception;
        }

        [Subject(typeof(Receipts))]
        public class when_confirming_multiple_receipts : WithSubject<Receipts>
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

        [Subject(typeof(Receipts))]
        public class when_registering_receipt : WithSubject<Receipts>
        {
            Because of = () =>
                Subject.RegisterReceipt(new ConfirmReceipt(300));

            It should_have_receipt_registered_for_receipt = () =>
                Subject.HasPendingConfirmation(300).ShouldBeTrue();
        }
    }
}
