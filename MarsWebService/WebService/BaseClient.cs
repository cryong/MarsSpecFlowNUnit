using System;
using System.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace MarsFramework.WebService
{
    public abstract class BaseClient
    {
        protected readonly IRestClient _client;
        protected readonly string _token;
        private readonly string _baseUrl;

        // no auth
        public BaseClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(baseUrl)
            {
                //ThrowOnAnyError = true
            };

        }

        public BaseClient(string baseUrl, string token) : this(baseUrl)
        {
            _client.Authenticator = new JwtAuthenticator(token);
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var response = _client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogError(_baseUrl, request, response);
            }
            return response;
        }

        public IRestResponse<T> Execute<T>(IRestRequest request)
        {
            var response = _client.Execute<T>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogError(_baseUrl, request, response);
            }

            return response;
        }

        public T Get<T>(IRestRequest request) where T : new()
        {
            var response = _client.Execute<T>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                LogError(_baseUrl, request, response);
            }
            return response.Data;
        }

        private void LogError(string baseUrl, IRestRequest request, IRestResponse response)
        {
            //Get the values of the parameters passed to the API
            string parameters = string.Join(", ", request.Parameters.Select(x => x.Name.ToString() + "=" + (x.Value ?? "NULL")).ToArray());

            //Set up the information message with the URL, 
            //the status code, and the parameters.
            string info = $"Request to {baseUrl}/{request.Resource} failed with status code {response.StatusCode}, "
                        + $"parameters {parameters}, and content: {response.Content}";
            //Acquire the actual exception
            if (response != null && response.ErrorException != null)
            {
                throw new Exception(info, response.ErrorException);
            }
            else
            {
                throw new Exception(info);
            }
        }
    }
}
