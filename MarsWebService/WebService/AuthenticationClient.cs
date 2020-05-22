using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using MarsWebService.Model;
using MarsFramework.WebService;

namespace MarsWebService.WebService
{
    public class AuthenticationClient : BaseClient
    {
        private readonly string _username;
        private readonly string _password;
        private const string key = "token";

        public AuthenticationClient(string username, string password) : base("http://localhost:60968/authentication/authentication")
        {
            _username = username;
            _password = password;
        }

        public string GetToken()
        {
            //Ensure.NotNull;
            var requestBody = new Credentials() { Username = _username, Password = _password };
            IRestRequest request = new RestRequest("signin", Method.POST)
                .AddJsonBody(JsonConvert.SerializeObject(requestBody));
            // need to handle error (status code)
            IRestResponse response = Execute(request);
            return JObject.Parse(response.Content)[key][key].ToString();
        }
    }
}
