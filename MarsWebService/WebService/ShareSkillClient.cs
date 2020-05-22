using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using MarsWebService.Model;

namespace MarsWebService.WebService
{
    public class ShareSkillClient : AbstractCRUDWSClient<ShareSkill>
    {
        public ShareSkillClient(string token) : base("http://localhost:51689/listing/listing", token)
        {
        }

        public IList<ShareSkill> ReadData(int offset, int limit = 5)
        {
            IRestRequest request = new RestRequest("getMultipleServiceListing", Method.POST)
                                    .AddJsonBody(JsonConvert.SerializeObject(new JObject(
                                        new JProperty("offset", offset),
                                        new JProperty("limit", limit))));
            var response = Execute(request);
            return JsonConvert.DeserializeObject<List<ShareSkill>>(JObject.Parse(response.Content)["data"].ToString(), new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

        }

        public override string AddData(ShareSkill skill)
        {
            Console.WriteLine(JsonConvert.SerializeObject(skill));
            IRestRequest request = new RestRequest("addListing", Method.POST).AddJsonBody(JsonConvert.SerializeObject(skill));
            var response = Execute(request);
            
            return JObject.Parse(response.Content)["id"].ToString();
        }

        public override void DeleteData(string id)
        {
            IRestRequest request = new RestRequest("deleteServiceListing", Method.POST).AddJsonBody(id);
            Execute(request);

        }

        public override IList<ShareSkill> ReadData()
        {
            return ReadData(0, 5);
        }
    }
}
