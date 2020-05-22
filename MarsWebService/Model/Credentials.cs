using Newtonsoft.Json;

namespace MarsWebService.Model
{
    public class Credentials
    {
        [JsonProperty(PropertyName ="Email")]
        public string Username { get; set; }
        [JsonProperty]
        public string Password { get; set; }

        public override string ToString()
        {
            return $"Username = '{Username}', Password = '{Password}'";
        }
    }
}
