using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using MarsWebService.Model;

namespace MarsWebService.WebService
{
    public class SkillClient : AbstractCRUDWSClient<Skill>
    {
        public SkillClient(string token) : base("http://localhost:60190/profile/profile", token)
        {
        }

        public override IList<Skill> ReadData()
        {
            IRestRequest request = new RestRequest("getSkill", Method.GET);
            return Get<List<Skill>>(request);
        }

        public override string AddData(Skill skill)
        {
            Console.WriteLine(JsonConvert.SerializeObject(skill));
            IRestRequest request = new RestRequest("addSkill", Method.POST)
                                    .AddJsonBody(JsonConvert.SerializeObject(skill));
            IRestResponse response = Execute(request);
            return JObject.Parse(response.Content)["id"].ToString();
        }

        public override void DeleteData(string id)
        {
            IRestRequest request = new RestRequest("deleteSkill", Method.POST)
                                    .AddJsonBody(JsonConvert.SerializeObject(new JObject(new JProperty("personSkillId", id))));
            Execute(request);

        }
    }
}
