using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MarsFramework.Model.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MarsWebService.Model
{
    public class ShareSkill : SearchableItem
    {
        public enum ServiceTypeOption
        {
            [Description("Hourly basis service")]
            HourlyBasisService,
            [Description("One-off service")]
            OneOffService
        }

        public enum LocationTypeOption
        {
            [Description("On-site")]
            OnSite,
            [Description("Online")]
            Online
        }

        public enum SkillTradeOption
        {
            [Description("Skill-exchange")]
            SkillExchange,
            [Description("Credit")]
            Credit
        }

        public enum ActiveOption
        {
            [Description("Active")]
            Active,
            [Description("Hidden")]
            Hidden
        }
        //[JsonIgnore]
        //[DeserializeAs]
        public override string Id { get; set; } = "0";
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Title { get; set; }
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Description { get; set; }
        [JsonIgnore]
        public string Category
        {
            get
            {
                if (_categoryId == null)
                {
                    return null;
                }

                foreach (KeyValuePair<string, string> entry in categoryMap)
                {
                    if (_categoryId == entry.Value)
                    {
                        return entry.Key;
                    }
                }

                return null;
            }
            set
            {
                categoryMap.TryGetValue(value, out string _hiddenValue);
                _categoryId = _hiddenValue;
            }
        }
        [JsonProperty(PropertyName = "categoryId")]
        private string _categoryId { get; set; }

        [JsonIgnore]
        public string SubCategory
        {
            get
            {
                if (_subCategoryId == null || _categoryId == null)
                {
                    return null;
                }

                foreach (KeyValuePair<string, string> entry in subCategoryMap[_categoryId])
                {
                    if (_subCategoryId == entry.Value)
                    {
                        return entry.Key;
                    }
                }
                return null;
            }
            set
            {
                if (_categoryId == null || value == null)
                {
                    _subCategoryId = null;
                    return;
                }

                subCategoryMap.TryGetValue(_categoryId, out var _hiddenMap);
                if (_hiddenMap == null)
                {
                    _subCategoryId = null;
                    return;
                }

                _hiddenMap.TryGetValue(value, out var _hiddenValue);
                _subCategoryId = _hiddenValue;
            }
        }
        [JsonProperty(PropertyName = "subcategoryId")]
        private string _subCategoryId { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public IEnumerable<Tag> TagsList { get; set; } = Enumerable.Empty<Tag>();
        [JsonIgnore]
        private string tags;
        [JsonIgnore]
        public string Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                TagsList = tags.Split(',').Select(s =>
                {
                    var trimmed = s.Trim();
                    return new Tag() { Id = trimmed, Text = trimmed };
                });
            }
        }
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public ServiceTypeOption ServiceType { get; set; } = ServiceTypeOption.HourlyBasisService;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public LocationTypeOption LocationType { get; set; } = LocationTypeOption.Online;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        [CustomJSONConverter]
        public Availability Availability { get; set; }
        [JsonIgnore]
        public SkillTradeOption SkillTrade
        {
            get { return isTradeable ? SkillTradeOption.SkillExchange : SkillTradeOption.Credit; }
            set
            {
                if (value == SkillTradeOption.SkillExchange)
                {
                    isTradeable = true;
                }
            }
        }
        [JsonProperty]
        private bool isTradeable;
        [JsonProperty(PropertyName = "skillTrade")]
        public IEnumerable<Tag> SkillExchangesList { get; set; } = Enumerable.Empty<Tag>();
        [JsonIgnore]
        private string skillExchanges;
        [JsonIgnore]
        public string SkillExchanges
        {
            get { return skillExchanges; }
            set
            {

                skillExchanges = value;
                SkillExchangesList = SkillExchanges.Split(',').Select(s =>
                {
                    var trimmed = s.Trim();
                    return new Tag() { Id = trimmed, Text = trimmed };
                });
            }
        }
        [JsonProperty(PropertyName = "charge")]
        public decimal Credit { get; set; }
        [JsonIgnore]
        public string WorkSamplePath { get; set; }
        [JsonIgnore]
        public ActiveOption Active
        {
            get
            {
                return isActive ? ActiveOption.Active : ActiveOption.Hidden;
            }
            set
            {
                if (value == ActiveOption.Active)
                {
                    isActive = true;
                }
            }
        }
        [JsonProperty]
        private bool isActive;

        [JsonIgnore]
        private readonly IDictionary<string, string> categoryMap = new Dictionary<string, string>
        {
            { "Graphics & Design", "1"}, { "Digital Marketing", "2"},
            { "Writing & Translation", "3"},{ "Video & Animation", "4"},
            { "Music & Audio", "5"},{ "Programming & Tech", "6"},
            { "Business", "7"},{ "Fun & Lifestyle", "8"}
        };

        // quick and dirty fix, come back and try to do it properly if there is time
        [JsonIgnore]
        private readonly Dictionary<string, Dictionary<string, string>> subCategoryMap = new Dictionary<string, Dictionary<string, string>>
        {
            { "1", new Dictionary<string, string> {
            { "Logo Design", "1"}, { "Book & Album covers", "2"},
            { "Flyers & Brochures", "3"},{ "Web & Mobile Design", "4"},
            { "Search & Display Marketing", "5"}
            }},

            { "2", new Dictionary<string, string> {
            { "Social Media Marketing", "1"},
            { "Content Marketing", "2"},{ "Video Marketing", "3"},
            { "Email Marketing", "4"}, { "Search & Display Marketing", "5"},
            }},

            { "3", new Dictionary<string, string> {
            { "Resumes & Cover Letters", "1"},{ "Proof Reading & Editing", "2"},
            { "Translation", "3"},
            { "Creative Writing", "4"}, { "Business Copywriting", "5"},
            } },

            { "4", new Dictionary<string, string> {
            { "Promotional Videos", "1"},{ "Editing & Post Production", "2"},
            { "Lyric & Music Videos", "3"},
            { "Other", "4"},
            }},

            { "5", new Dictionary<string, string> {
            { "Mixing & Mastering", "1"},
            { "Voice Over", "2"},{ "Song Writers & Composers", "3"},
            { "Other", "4"},
            }},

            { "6", new Dictionary<string, string> {
            { "WordPress", "1"}, { "Web & Mobile App", "2"},
            { "Data Analysis & Reports", "3"},{ "QA", "4"},
            { "Databases", "5"}, { "Other", "6"},
            }},

            { "7", new Dictionary<string, string> {
            { "Business Tips", "1"}, { "Presentations", "2"},
            { "Market Advice", "3"},{ "Legal Consulting", "4"},
            { "Financial Consulting", "5"}, { "Other", "6"},
            }},

            { "8", new Dictionary<string, string> {
            { "Online Lessons", "1"}, { "Relationship Advice", "2"},
            { "Astrology", "3"},{ "Health, Nutrition & Fitness", "4"},
            { "Gaming", "5"}, { "Other", "6"}
            }}
        };

        public override bool Equals(object obj)
        {
            // this should be enough for now
            if (obj is ShareSkill skill)
            {
                // if both object has id then use it otherwise compare title
                if (Id != null && skill.Id != null)
                {
                    return Id == skill.Id;
                }

                // current application functionality limits me from checking only these 3 proeprties
                return Title == skill.Title &&
                                   Description == skill.Description &&
                                   Category == skill.Category;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 2013830575;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Category);
            return hashCode;
        }
    }

    public class Tag
    {
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Id { get; set; }
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string Text { get; set; }
    }

    public class Availability
    {
        [JsonIgnore]
        private DateTime? _startDate;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string StartDate
        {
            get { return _startDate?.ToString("dd-MM-yyyy"); }
            set
            {
                _startDate = DateTime.Parse(value);
            }
        }
        [JsonIgnore]
        public DateTime? _endDate;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string EndDate
        {
            get { return _endDate?.ToString("dd-MM-yyyy"); }
            set
            {
                // null check
                if (value == null)
                {
                    return;
                }
                _endDate = DateTime.Parse(value);
            }
        }

        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public IList<DayEntry> DayEntries = new List<DayEntry>();
    }

    public class DayEntry
    {
        [JsonIgnore]
        public DayOfWeek Day { get; set; }
        [JsonIgnore]
        private DateTime? _startTime;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string StartTime
        {
            get
            {
                return _startTime?.ToString("HH:mm") ?? "";
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                _startTime = DateTime.Parse(value);
            }
        }

        [JsonIgnore]
        public string ReadableStartTime => _startTime?.ToString("hh:mmtt") ?? ""; //  to be used for entering data on screen
        [JsonIgnore]
        private DateTime? _endTime;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public string EndTime
        {
            get
            {
                return _endTime?.ToString("HH:mm") ?? "";
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                _endTime = DateTime.Parse(value);
            }
        }

        [JsonIgnore]
        public string ReadableEndTime => _endTime?.ToString("hh:mmtt") ?? ""; //  to be used for entering data on screen

        [JsonProperty(PropertyName = "Available")]
        public bool IsAvailable { get; set; } = false;
    }
}