using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace MarsWebService.Model
{
    public class Certification : SearchableItem
    {
        [DeserializeAs(Name = "id")]
        [JsonProperty(PropertyName = "id")]
        public override string Id { get; set; } = "0"; // automatic assignment when persisted
        [DeserializeAs(Name = "certificationName")]
        [JsonProperty(PropertyName = "certificationName")]
        public string Name { get; set; }
        [DeserializeAs(Name = "certificationFrom")]
        [JsonProperty(PropertyName = "certificationFrom")]
        public string Organisation { get; set; }
        [DeserializeAs(Name = "certificationYear")]
        [JsonProperty(PropertyName = "certificationYear")]
        public string Year { get; set; }

        public Certification()
        {

        }
        public Certification(string name, string organisation, string year)
        {
            Name = name;
            Organisation = organisation;
            Year = year;
        }

        public override bool Equals(object obj)
        {
            return obj is Certification certification &&
                   Name == certification.Name &&
                   Organisation == certification.Organisation &&
                   Year == certification.Year;
        }

        public override int GetHashCode()
        {
            var hashCode = 1118675265;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Organisation);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Year);
            return hashCode;
        }
    }
}
