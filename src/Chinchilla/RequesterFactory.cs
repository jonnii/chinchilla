using Chinchilla.Logging;

namespace Chinchilla
{
    public class RequesterFactory : TrackableFactory<Trackable>, IRequesterFactory
    {
        private readonly ILogger logger = Logger.Create<RequesterFactory>();

        public IRequester<TRequest, TResponse> Create<TRequest, TResponse>(IBus bus)
            where TRequest : ICorrelated
            where TResponse : ICorrelated
        {
            logger.InfoFormat(
                "Creating Requester for {0} responding to {1}",
                typeof(TRequest).Name,
                typeof(TResponse).Name);

            var requester = new Requester<TRequest, TResponse>(bus);

            requester.Start();

            Track(requester);

            return requester;
        }
    }
}