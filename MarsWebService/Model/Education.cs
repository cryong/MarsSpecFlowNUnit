using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Deserializers;

namespace MarsWebService.Model
{
    public class Education : SearchableItem
    {
        [JsonProperty(PropertyName = "id")]
        public override string Id { get; set; } = "0"; // automatic assignment when persisted
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string InstituteName { get; set; }
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string DegreeTitle { get; set; }
        [JsonProperty(PropertyName = "degree")]
        public string DegreeName { get; set; }
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string YearOfGraduation { get; set; }

        public Education()
        {

        }

        public Education(string country, string instituteName, string degreeTitle, string degreeName, string yearOfGraduation)
        {
            Country = country;
            InstituteName = instituteName;
            DegreeTitle = degreeTitle;
            DegreeName = degreeName;
            YearOfGraduation = yearOfGraduation;
        }

        public override bool Equals(object obj)
        {
            return obj is Education education &&
                   InstituteName == education.InstituteName &&
                   Country == education.Country &&
                   DegreeTitle == education.DegreeTitle &&
                   DegreeName == education.DegreeName &&
                   YearOfGraduation == education.YearOfGraduation;
        }

        public override int GetHashCode()
        {
            var hashCode = 460369524;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InstituteName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DegreeTitle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DegreeName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(YearOfGraduation);
            return hashCode;
        }
    }
}
