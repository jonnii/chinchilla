using System.Collections.Generic;
using Chinchilla.Api.Extensions;
using System.Collections;
using System.Net;
using System;
using System.IO;
using System.Text;

namespace Chinchilla.Api
{
    public class HttpClient
    {
        private readonly string root;

        public HttpClient(string root)
        {
            this.root = root;
        }

        public HttpWebResponse Execute(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            string method = "GET",
            object body = null
            )
        {
            var request = CreateRequest(resource, parameters, method, body);
            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                return (HttpWebResponse)e.Response;
            }
        }

        public T Execute<T>(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters = null,
            string method = "GET",
            object body = null
            ) where T : new()
        {
            var request = CreateRequest(resource, parameters, method, body);
            var response = (HttpWebResponse)request.GetResponse();

            return DeserializeResponse<T>(response);
        }

        private static T DeserializeResponse<T>(HttpWebResponse response)
        {
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                SimpleJson.CurrentJsonSerializerStrategy = new RabbitJsonSerializerStrategy();
                return SimpleJson.DeserializeObject<T>(sr.ReadToEnd());
            }
        }

        private HttpWebRequest CreateRequest(
            string resource,
            IEnumerable<KeyValuePair<string, string>> parameters,
            string method,
            object body
            )
        {
            var parametersTable = ParametersToHastable(parameters);
            var formattedResource = FormatResource(resource, parametersTable);
            var uri = CombineUri(root, formattedResource);

            var request = WebRequest.CreateHttp(uri);
            request.Method = method;
            request.Accept = string.Empty;
            request.ContentType = "application/json; charset=utf-8";

            SetBasicAuthHeader(request, "guest", "guest");

            if (body != null)
            {
                var serializedBody = SimpleJson.SerializeObject(body, new RabbitJsonSerializerStrategy());
                var bodyData = Encoding.UTF8.GetBytes(serializedBody);
                request.GetRequestStream().Write(bodyData, 0, bodyData.Length);
                request.GetRequestStream().Close();
            }
            return request;
        }

        private static Uri CombineUri(string root, string resource)
        {
            return new Uri(root + "/" + resource);
        }

        private static string FormatResource(string resource, Hashtable parameters)
        {
            return resource.Inject(parameters);
        }

        private static Hashtable ParametersToHastable(IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var result = new Hashtable();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    result.Add(parameter.Key, parameter.Value);
                }
            }
            return result;
        }

        private static void SetBasicAuthHeader(WebRequest request, string user, string password)
        {
            var authInfo = user + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }


}