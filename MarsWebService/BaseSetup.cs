using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MarsWebService
{
    // use this class for data set up and clean up
    public class BaseSetup : RestClient
    {
        // https://exceptionnotfound.net/building-the-ultimate-restsharp-client-in-asp-net-and-csharp/

        // https://github.com/CodeMazeBlog/ConsumeRestfulApisExamples/tree/master/RESTfulAPIConsume
        // https://github.com/App-vNext/Polly
        //https://github.com/twilio/twilio-csharp/blob/master/src/Twilio/JWT/Client/ClientCapability.cs
        //https://github.com/MelbourneDeveloper/RestClient.Net/blob/master/RestClient.Net/ClientFactory.cs
        public BaseSetup(string baseURI)
        {

        }


        public string GetToken()
        {
            var client = new RestClient("http://localhost:60968/authentication/authentication");
            client.Timeout = -1;
            var request = new RestRequest("signin", Method.POST);  // http://localhost:60968/authentication/authentication/signin
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\n\t\"Email\": \"changhoon.ryong@gmail.com\",\n\t\"Password\": \"testm3\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //var requestbody = new JObject(new JProperty("Email", _username), new JProperty("Password", _password));
            //                //.AddHeader("Content-Type", ContentType.Json)// not sure if needed if we set it in parameter?
            //.AddParameter(new Parameter(ContentType.Json, requestbody, ParameterType.RequestBody))
            //.AddParameter(ContentType.Json, reqBody, ParameterType.RequestBody);


            JObject responseObject = JObject.Parse(response.Content);
            string JWTstring = responseObject["token"]["token"].ToString();
            Console.Write(JWTstring);
            Console.Write("======================================================================================");
            return JWTstring;
        }

        public void DoSetUp()
        {

            string JWTstring = GetToken();
            // reuqest once for test run


            //client = new RestClient("http://localhost:51689/listing/listing/getMultipleServiceListing");
            //client.Timeout = -1;
            //request = new RestRequest(Method.POST);
            //request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", $"Bearer {JWTstring}");
            //request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json,application/json", "{\n\t\"offset\":0,\n\t\"limit\":10\n}", ParameterType.RequestBody);
            //response = client.Execute(request);
            //Console.Write("======================================================================================");
            //Console.WriteLine(response.Content);
            //Console.Write("======================================================================================");
            //// need to perform set up here


            IList <JObject> skillsToAdd = new List<JObject> {
                new JObject(new JProperty("PersonSkillId", 0), new JProperty("Skill", "aa"), new JProperty("ExperienceLevel", "Beginner")),
                new JObject(new JProperty("PersonSkillId", 0), new JProperty("Skill", "bb"), new JProperty("ExperienceLevel", "Beginner")),
                new JObject(new JProperty("PersonSkillId", 0), new JProperty("Skill", "cc"), new JProperty("ExperienceLevel", "Beginner"))};


            var client = new RestClient();
            var request = new RestRequest();
            IRestResponse response;
            foreach (var x in skillsToAdd)
            {
                client = new RestClient("http://localhost:60190/profile/profile");
                client.Timeout = -1;
                request = new RestRequest("AddSkill", Method.POST);
                request.AddHeader("Authorization", $"Bearer {JWTstring}");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", x, ParameterType.RequestBody);
                response = client.Execute(request);
                Console.WriteLine(response.Content);
                // fail message example below
                //{"success":false,"message":"This skill is already exist in your skill list."}
            }

            client = new RestClient("http://localhost:60190/profile/profile");
            client.Timeout = -1;
            request = new RestRequest("getSkill", Method.GET);
            request.AddHeader("Authorization", $"Bearer {JWTstring}");
            response = client.Execute(request);
            Console.WriteLine(response.Content);
            JArray skillObjects = JArray.Parse(response.Content);
            IList<JObject> skillObjectList = skillObjects.Select(x => new JObject(new JProperty("personSkillId", x["personSkillId"]))).ToList();

            foreach (var j in skillObjectList)
            {
                //Console.WriteLine($"###############{j}");
                client = new RestClient("http://localhost:60190/profile/profile");
                client.Timeout = -1;
                request = new RestRequest("deleteSkill", Method.POST);
                request.AddHeader("Authorization", $"Bearer {JWTstring}");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", j, ParameterType.RequestBody);
                response = client.Execute(request);
                Console.WriteLine(response.Content);
            }
        }


    }
}
