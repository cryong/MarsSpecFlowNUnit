using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Deserializers;

namespace MarsWebService.Model
{
    public class Skill : SearchableItem
    {

        [DeserializeAs(Name = "personSkillId")]
        [JsonProperty(PropertyName = "personSkillId")]
        public override string Id { get; set; } = "0"; // automatic assignment when persisted

        [DeserializeAs(Name="Skill")]
        [JsonProperty(PropertyName = "Skill")]
        public string Name { get; set; }

        [DeserializeAs(Name = "ExperienceLevel")]
        [JsonProperty(PropertyName = "ExperienceLevel")]
        public string Level { get; set; }

        public Skill()
        {

        }

        public Skill(string name, string level)
        {
            Name = name;
            Level = level;
        }

        public override bool Equals(object obj)
        {
            return obj is Skill skill &&
                   Name == skill.Name &&
                   Level == skill.Level;
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
