using System.Threading.Tasks;
using Chinchilla.Integration.Features.Consumers;
using Chinchilla.Integration.Features.Messages;
using Xunit;

namespace Chinchilla.Integration.Features
{
    public class RequestResponseFeature : Feature
    {
        [Fact]
        public async Task ShouldCreateRequestResponseOnBus()
        {
            using (var bus = await CreateBus())
            {
                bus.Subscribe(new CapitalizeMessageConsumer());

                CapitalizedMessage capitalized = null;
                bus.Request<CapitalizeMessage, CapitalizedMessage>(
                    new CapitalizeMessage("where am i?"),
                    response => { capitalized = response; });

                await WaitFor(() => capitalized != null);

                Assert.NotNull(capitalized);
                Assert.Equal("WHERE AM I?", capitalized.Capitalized);
            }
        }

        //[Fact]
        //public async Task ShouldSupportAsyncForBasicRequestResponse()
        //{
        //    using (var bus = await CreateBus())
        //    {
        //        bus.Subscribe(new CapitalizeMessageConsumer());

        //        var response = await bus.RequestAsync<CapitalizeMessage, CapitalizedMessage>(
        //            new CapitalizeMessage("where am i?"));

        //        await WaitFor(() => response != null);

        //        Assert.NotNull(response);
        //        Assert.Equal("WHERE AM I?", response.Capitalized);
        //    }
        //}

        //[Fact]
        //public async Task ShouldCreateRequestResponseWithRequester()
        //{
        //    using (var bus = await CreateBus())
        //    {
        //        using (var requester = bus.CreateRequester<CapitalizeMessage, CapitalizedMessage>())
        //        {
        //            bus.Subscribe(new CapitalizeMessageConsumer());

        //            CapitalizedMessage capitalized = null;

        //            requester.Request(
        //                new CapitalizeMessage("where am i?"),
        //                response => { capitalized = response; });

        //            await WaitFor(() => capitalized != null);

        //            Assert.NotNull(capitalized);
        //            Assert.Equal("WHERE AM I?", capitalized.Capitalized);
        //        }
        //    }
        //}

        //[Fact]
        //public async Task ShouldSupportAsyncForRequesterRequestResponse()
        //{
        //    using (var bus = await CreateBus())
        //    {
        //        using (var requester = bus.CreateRequester<CapitalizeMessage, CapitalizedMessage>())
        //        {
        //            bus.Subscribe(new CapitalizeMessageConsumer());

        //            var response = await requester.RequestAsync(
        //                new CapitalizeMessage("where am i?"));

        //            var capitalized = response.Capitalized;

        //            await WaitFor(() => capitalized != null);

        //            Assert.NotNull(capitalized);
        //            Assert.Equal("WHERE AM I?", capitalized);
        //        }
        //    }
        //}
    }
}
