using Chinchilla.Serializers;
using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications.Serializers
{
    public class DefaultMessageTypeFactorySpecification
    {
        [Subject(typeof(DefaultMessageTypeFactory))]
        public class in_general : WithSubject<DefaultMessageTypeFactory>
        {
            It should_not_have_cached_factory_by_default = () =>
                Subject.HasCachedFactory(typeof(IMagicMessage)).ShouldBeFalse();
        }

        [Subject(typeof(DefaultMessageTypeFactory))]
        public class when_getting_interface_instance : WithSubject<DefaultMessageTypeFactory>
        {
            Because of = () =>
                message = (IMagicMessage)Subject.GetTypeFactory(typeof(IMagicMessage))();

            It should_create_magic_message_instance = () =>
                message.ShouldBeAssignableTo<IMagicMessage>();

            It should_have_cached_create_delegate_for_message_type = () =>
                Subject.HasCachedFactory(typeof(IMagicMessage)).ShouldBeTrue();

            static IMagicMessage message;
        }

        public interface IMagicMessage
        {
            string Name { get; set; }
        }
    }
}
