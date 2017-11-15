using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class PausableWorkersFeature : Feature
    {
        [Fact]
        public async Task ShouldBeAbleToPauseWorker()
        {
            using (var bus = await CreateBus())
            {
                var seen = 0;

                var subscription = bus.Subscribe<HelloWorldMessage>(
                    m => Interlocked.Increment(ref seen),
                    c => c.DeliverUsing<WorkerPoolDeliveryStrategy>());

                // pause the worker
                var state = subscription.State;
                var worker = state.Workers.Single();
                subscription.Workers.Pause(worker.Name);

                // publish a message to the queue
                bus.Publish(new HelloWorldMessage());

                await Task.Delay(100);

                // we shouldn't have processed the message and the worker should be paused
                Assert.Equal(0, seen);
                state = subscription.State;
                Assert.Equal(WorkerStatus.Paused, state.Workers.First().Status);

                // resume the worker
                subscription.Workers.Resume(worker.Name);

                await WaitFor(() => seen == 1);

                Assert.Equal(1, seen);

                state = subscription.State;
                Assert.NotEqual(WorkerStatus.Paused, state.Workers.First().Status);
            }
        }
    }
}
