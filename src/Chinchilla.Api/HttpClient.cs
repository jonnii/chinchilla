using System.Collections.Generic;
using RestSharp;

namespace Chinchilla.Api
{
    public class HttpClient
    {
        private readonly IRestClient restClient;

        public HttpClient(string root)
        {
            restClient = new RestClient(root)
                         {
                             Authenticator = new HttpBasicAuthenticator("guest", "guest")
                         };
            restClient.AddDefaultHeader("Content-Type", "application/json; charset=utf-8");
        }

        public IRestResponse Execute(
            string resource, 
            IEnumerable<KeyValuePair<string, string>> parameters = null, 
            Method method = Method.GET,
            object body = null
            )
        {
            var request = CreateRequest(resource, parameters, method, body);
            var response = restClient.Execute(request);

            return response;
        }

        public T Execute<T>(
            string resource, 
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            Method method = Method.GET,
            object body = null
            ) where T : new()
        {
            var request = CreateRequest(resource, parameters, method, body);
            var response = restClient.Execute<T>(request);

            return response.Data;
        }

        private static IRestRequest CreateRequest(
            string resource, 
            IEnumerable<KeyValuePair<string, string>> parameters = null, 
            Method method = Method.GET,
            object body = null
            )
        {
            var request = new RestRequest(resource, method)
                          {
                              JsonSerializer = new RabbitJsonSerializerStrategy(),
                              RequestFormat = DataFormat.Json
                          };
            request.AddHeader("Accept", string.Empty);
            if (body != null)
            {
                request.AddBody(body);
            }
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.AddUrlSegment(parameter.Key, parameter.Value);
                }
            }

            return request;
        }
    }
}