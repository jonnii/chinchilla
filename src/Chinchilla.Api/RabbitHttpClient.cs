using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Chinchilla.Api.Extensions;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chinchilla.Api
{
    public class RabbitHttpClient
    {
        private readonly string root;

        private readonly string username;

        private readonly string password;

        private readonly HttpClient client;

        private readonly JsonSerializer serializer;

        public RabbitHttpClient(string root, string username, string password)
        {
            this.root = root;
            this.username = username;
            this.password = password;

            client = new HttpClient();

            serializer = new JsonSerializer
            {
                ContractResolver = LowercasePropertyContractResolver.Instance
            };
        }

        public Task<HttpResponseMessage> Execute(
            HttpMethod method,
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            object body = null)
        {
            var request = CreateRequest(resource, parameters, method, body);

            return client.SendAsync(request);
        }

        public async Task<T> Execute<T>(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            object body = null)
        {
            var request = CreateRequest(resource, parameters, HttpMethod.Get, body);

            var response = await client.SendAsync(request);
            return await DeserializeResponse<T>(response);
        }

        public async Task<IEnumerable<T>> ExecuteList<T>(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            object body = null)
        {
            var request = CreateRequest(resource, parameters, HttpMethod.Get, body);

            var response = await client.SendAsync(request);
            return await DeserializeResponse<List<T>>(response);
        }

        public async Task<IEnumerable<T>> ExecuteList<T>(
            HttpMethod method,
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            object body = null)
        {
            var request = CreateRequest(resource, parameters, method, body);

            var response = await client.SendAsync(request);
            return await DeserializeResponse<List<T>>(response);
        }

        public async Task<T> Execute<T>(
            HttpMethod method,
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            object body = null)
        {
            var request = CreateRequest(resource, parameters, method, body);

            var response = await client.SendAsync(request);
            return await DeserializeResponse<T>(response);
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStreamAsync();

            using (var streamReader = new StreamReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        private HttpRequestMessage CreateRequest(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters,
            HttpMethod method,
            object body)
        {
            var parametersTable = ParametersToHastable(parameters);
            var formattedResource = resource.Inject(parametersTable);
            var uri = new Uri(root + "/" + formattedResource);

            var request = new HttpRequestMessage(method, uri);

            request.Headers.Add("Accept", string.Empty);
            request.Headers.Add("Authorization", BuildAuthHeader(username, password));

            if (body != null)
            {
                var serializedBody = JsonConvert.SerializeObject(body,
                    new JsonSerializerSettings
                    {
                        ContractResolver = LowercasePropertyContractResolver.Instance
                    });

                request.Content = new StringContent(serializedBody, Encoding.UTF8, "application/json");
            }
            else if (method != HttpMethod.Get)
            {
                request.Content = new ByteArrayContent(new byte[0]);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return request;
        }

        private Dictionary<string, string> ParametersToHastable(IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var result = new Dictionary<string, string>();

            if (parameters == null)
            {
                return result;
            }

            foreach (var parameter in parameters)
            {
                result.Add(parameter.Key, parameter.Value);
            }

            return result;
        }

        private string BuildAuthHeader(string user, string password)
        {
            var authInfo = user + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
            return "Basic " + authInfo;
        }

        public class LowercasePropertyContractResolver : DefaultContractResolver
        {
            public static readonly LowercasePropertyContractResolver Instance = new LowercasePropertyContractResolver();

            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLowerInvariant();
            }
        }
    }
}