using Chinchilla.Api.Extensions;
using Machine.Specifications;

namespace Chinchilla.Api.Specifications.Extensions
{
    public class StringExtensionsSpecification
    {
        [Subject(typeof(StringExtensions))]
        public class in_general : with_replacements_reset
        {
            It should_have_built_in_replacements = () =>
            {
                var replacements = new[] { "{user.username}", "{machine.name}" };

                foreach (var expected in replacements)
                {
                    StringExtensions.HasReplacement(expected).ShouldBeTrue();
                }
            };
        }

        [Subject(typeof(StringExtensions))]
        public class when_adding_replacement : with_replacements_reset
        {
            Because of = () =>
                StringExtensions.AddReplacement("{foo}", () => "bar");

            It should_have_replacement = () =>
                StringExtensions.HasReplacement("{foo}").ShouldBeTrue();
        }

        [Subject(typeof(StringExtensions))]
        public class when_formatting_without_replacements : with_replacements_reset
        {
            Because of = () =>
                result = "foobarzomg".FormatWithReplacements();

            It should_not_transform = () =>
                result.ShouldEqual("foobarzomg");

            static string result;
        }

        [Subject(typeof(StringExtensions))]
        public class when_running_with_replacements : with_replacements_reset
        {
            Establish context = () =>
            {
                StringExtensions.AddReplacement("{foo}", () => "a");
                StringExtensions.AddReplacement("{zom}", () => "b");
            };

            Because of = () =>
                result = "{foo}{zom}".FormatWithReplacements();

            It should_run_all_replacements = () =>
                result.ShouldEqual("ab");

            static string result;
        }

        public class with_replacements_reset
        {
            Establish context = () =>
                StringExtensions.ResetReplacements();
        }
    }
}
