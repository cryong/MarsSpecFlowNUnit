using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MarsWebService.Model;
using RestSharp;

namespace MarsWebService.WebService
{
    public class CertificationClient : AbstractCRUDWSClient<Certification>
    {
        public CertificationClient(string token) : base("http://localhost:60190/profile/profile", token)
        {
        }

        public override IList<Certification> ReadData()
        {
            IRestRequest request = new RestRequest("getCertification", Method.GET);
            return Get<List<Certification>>(request);
        }

        public override string AddData(Certification skill)
        {
            Console.WriteLine(JsonConvert.SerializeObject(skill));
            IRestRequest request = new RestRequest("addCertification", Method.POST).AddJsonBody(JsonConvert.SerializeObject(skill));
            IRestResponse response = Execute(request);
            return JObject.Parse(response.Content)["id"].ToString();
        }

        public override void DeleteData(string id)
        {
            IRestRequest request = new RestRequest("deleteCertification", Method.POST)
                                    .AddJsonBody(JsonConvert.SerializeObject(new JObject(new JProperty("id", id))));
            Execute(request);

        }
    }
}
