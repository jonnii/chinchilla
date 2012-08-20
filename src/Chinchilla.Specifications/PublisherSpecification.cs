using System;
using Chinchilla.Specifications.Messages;
using Machine.Fakes;
using Machine.Specifications;
using RabbitMQ.Client;

namespace Chinchilla.Specifications
{
    public class PublisherSpecification
    {
        [Subject(typeof(Publisher<>))]
        public class when_disposing : WithSubject<Publisher<TestMessage>>
        {
            Because of = () =>
                Subject.Dispose();

            It should_dispose_model = () =>
                The<IModelReference>().WasToldTo(m => m.Dispose());
        }

        [Subject(typeof(Publisher<>))]
        public class when_disposing_multiple_times : WithSubject<Publisher<TestMessage>>
        {
            Because of = () =>
            {
                Subject.Dispose();
                Subject.Dispose();
            };

            It should_dispose_model = () =>
                The<IModelReference>().WasToldTo(m => m.Dispose()).OnlyOnce();
        }

        [Subject(typeof(Publisher<>))]
        public class when_publishing : WithSubject<Publisher<TestMessage>>
        {
            Establish context = () =>
            {
                model = An<IModel>();
                properties = An<IBasicProperties>();

                The<IModelReference>().WhenToldTo(r => r.Execute(Param.IsAny<Action<IModel>>()))
                    .Callback<Action<IModel>>(act => act(model));

                The<IModelReference>().WhenToldTo(r => r.Execute(Param.IsAny<Func<IModel, IBasicProperties>>()))
                    .Return(properties);

                The<IMessageSerializer>().WhenToldTo(s => s.ContentType).Return("content-type");
            };

            Because of = () =>
                Subject.Publish(new TestMessage());

            It should_serialize_message = () =>
                The<IMessageSerializer>().WasToldTo(s => s.Serialize(Param.IsAny<IMessage<TestMessage>>()));

            It should_set_content_type_on_properties = () =>
                properties.ContentType.ShouldEqual("content-type");

            It should_route_message = () =>
                The<IRouter>().WasToldTo(r => r.Route(Param.IsAny<TestMessage>()));

            static IModel model;

            static IBasicProperties properties;
        }
    }
}
