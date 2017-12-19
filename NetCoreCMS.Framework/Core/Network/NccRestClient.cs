using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Network
{
    public class NccRestClient
    {
        public static ResponseT Get<ResponseT>(string url, string path = "", List<KeyValuePair<string,string>> paramiters = null, List<KeyValuePair<string, string>> headers = null)
        {
            (RestClient client, RestRequest request) = GetRestClientAndRequest(url, path, Method.GET, paramiters, headers);
            var response = new RestResponse();

            Task.Run(async () => {
                response = await GetResponseAsync(client, request) as RestResponse;
            }).Wait();

            var jsonResponse = JsonConvert.DeserializeObject<ResponseT>(response.Content);

            if(jsonResponse != null)
            {
                return jsonResponse;
            }

            return default(ResponseT);
        }
        
        public static string Get(string url, string path = "", List<KeyValuePair<string, string>> paramiters = null, List<KeyValuePair<string, string>> headers = null)
        {
            (RestClient client, RestRequest request) = GetRestClientAndRequest(url, path, Method.GET, paramiters, headers);
            var response = new RestResponse();

            Task.Run(async () => {
                response = await GetResponseAsync(client, request) as RestResponse;
            }).Wait();
            
            return response.Content;
        }

        public static ResponseT Post<ResponseT>(string url, string path = "", List<KeyValuePair<string, string>> paramiters = null, List<KeyValuePair<string, string>> headers = null)
        {

            (RestClient client, RestRequest request) = GetRestClientAndRequest(url, path, Method.POST, paramiters, headers);
            var response = new RestResponse();

            Task.Run(async () => {
                response = await GetResponseAsync(client, request) as RestResponse;
            }).Wait();

            var jsonResponse = JsonConvert.DeserializeObject<ResponseT>(response.Content);

            if (jsonResponse != null)
            {
                return jsonResponse;
            }

            return default(ResponseT);
        }
        
        public static string Post(string url, string path = "", List<KeyValuePair<string, string>> paramiters = null, List<KeyValuePair<string, string>> headers = null)
        {
            (RestClient client, RestRequest request) = GetRestClientAndRequest(url, path, Method.POST, paramiters, headers);
            var response = new RestResponse();

            Task.Run(async () => {
                response = await GetResponseAsync(client, request) as RestResponse;
            }).Wait();

            return response.Content;
        }

        #region Private Methods

        private static (RestClient client, RestRequest request) GetRestClientAndRequest(string url, string path="", Method method = Method.GET, List<KeyValuePair<string, string>> paramiters = null, List<KeyValuePair<string, string>> headers = null)
        {
            var client = new RestClient(new Uri(url));
            var request = new RestRequest(path, method);

            if (paramiters != null)
            {
                foreach (var item in paramiters)
                {
                    if (method == Method.POST) {
                        request.AddParameter(item.Key, item.Value);
                    }
                    else
                    {
                        request.AddQueryParameter(item.Key, item.Value);
                    }
                }
            }

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    request.AddHeader(item.Key, item.Value);
                }
            }
            var fullUrl = client.BuildUri(request);
            return (client, request);
        }

        public static Task<IRestResponse> GetResponseAsync(RestClient restClient, RestRequest restRequest)
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse>();
            restClient.ExecuteAsync(restRequest, response => {
                taskCompletionSource.SetResult(response);
            });

            return taskCompletionSource.Task;
        }

        #endregion

    }

}
