using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using MarsWebService.Model;

namespace MarsWebService.WebService
{
    public class EducationClient : AbstractCRUDWSClient<Education>
    {
        public EducationClient(string token) : base("http://localhost:60190/profile/profile", token)
        {
        }

        public override IList<Education> ReadData()
        {
            IRestRequest request = new RestRequest("getEducation", Method.GET);
            var response = Execute(request);
            return JsonConvert.DeserializeObject<List<Education>>(JObject.Parse(response.Content)["education"].ToString(), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public override string AddData(Education education)
        {
            Console.WriteLine(JsonConvert.SerializeObject(education));
            IRestRequest request = new RestRequest("addEducation", Method.POST).AddJsonBody(JsonConvert.SerializeObject(education));
            IRestResponse response = Execute(request);
            return JObject.Parse(response.Content)["id"].ToString();
        }

        public override void DeleteData(string id)
        {
            IRestRequest request = new RestRequest("deleteEducation", Method.POST).AddJsonBody(JsonConvert.SerializeObject(new JObject(new JProperty("id", id))));
            Execute(request);

        }
    }
}
