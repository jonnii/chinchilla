using Machine.Fakes;
using Machine.Specifications;

namespace Chinchilla.Specifications
{
    public class TaskWorkerSpecification
    {
        [Subject(typeof(TaskWorker))]
        public class in_general : WithSubject<TaskWorker>
        {
            It should_have_name = () =>
                Subject.Name.ShouldNotBeNull();
        }
    }
}
