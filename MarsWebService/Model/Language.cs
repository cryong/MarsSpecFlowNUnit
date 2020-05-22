using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace MarsWebService.Model
{
    public class Language : SearchableItem
    {
        [DeserializeAs(Name = "personLanguageId")]
        [JsonIgnore]
        public override string Id { get; set; } = "0"; // automatic assignment when persisted
        [DeserializeAs(Name = "language")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "level")]
        [DeserializeAs(Name = "languageLevel")]
        public string Level { get; set; }

        public Language()
        {

        }
        public Language(string name, string level) {
            Name = name;
            Level = level;
        }

        public override bool Equals(object obj)
        {
            return obj is Language language &&
                   Name == language.Name &&
                   Level == language.Level;
        }

        public override int GetHashCode()
        {
            var hashCode = 1635173235;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Level);
            return hashCode;
        }
    }
}
