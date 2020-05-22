using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using MarsWebService.Model;
using MarsFramework.WebService;

namespace MarsWebService.WebService
{
    public class LanguageClient : AbstractCRUDWSClient<Language>
    {
        public LanguageClient(string token) : base("http://localhost:60190/profile/profile", token)
        {
        }

        public override IList<Language> ReadData()
        {
            IRestRequest request = new RestRequest("getLanguage", Method.GET);
            return Get<List<Language>>(request);
        }

        public override string AddData(Language language)
        {
            Console.WriteLine(JsonConvert.SerializeObject(language));
            IRestRequest request = new RestRequest("addLanguage", Method.POST).AddJsonBody(JsonConvert.SerializeObject(language));
            IRestResponse response = Execute(request);
            return JObject.Parse(response.Content)["id"].ToString();
        }

        public override void DeleteData(string id)
        {
            IRestRequest request = new RestRequest("deleteLanguage", Method.POST).AddJsonBody(JsonConvert.SerializeObject(new JObject(new JProperty("id", id))));
            Execute(request);

        }
    }
}
