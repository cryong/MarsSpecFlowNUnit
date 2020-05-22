using System;
using MarsWebService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using MarsWebService.WebService;
using System.Linq;
using MarsFramework.Service;

namespace NUnitTest
{
    public class Test
    {
        public enum TestEnum
        {
            TEST,
            YES,
            NO
        }
        private string what;
        [JsonProperty(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        //[JsonConverter(typeof(StringEnumConverter))]
        public TestEnum TestEnums { get; set; }
    }

    //[TestClass]
    //[TestFixture]
    public class LoginTest : TestBase
    {
        //[Test]
        public void test_converstion()
        {
            //Type rowType = row.GetType();
            //Enum.TryParse(rowType.Name, out ProfileInfoType detailType);
            IEnumerable<SearchableItem> test = new List<SearchableItem> { new Language() { Name= "A" }, new Language() { Name = "B" }, new Language() { Name = "C" } };

            //Type elementType = test.GetType().GetGenericArguments()[0];
            Type elementType = test.ElementAt(0).GetType();
            var methodInfo = typeof(Enumerable).GetMethod("Cast");
            var genericMethod = methodInfo.MakeGenericMethod(elementType);
            System.Collections.IEnumerable converted = genericMethod.Invoke(null, new[] { test }) as System.Collections.IEnumerable;


            Console.WriteLine();
            Console.WriteLine(test.ElementAt(0).GetType());
            Console.WriteLine(converted.GetType());
        }

        //[Test]
        public void test_test()
        {

            var x = new Test()
            {
                TestEnums = Test.TestEnum.NO
            };
            Console.WriteLine(JsonConvert.SerializeObject(x));
        }

        //[Test]
        public void Login_ValidCredentials_SuccessfulLogin()
        {
            ExtentTestManager.CreateTest("yo testing");
            //new SignIn(Driver).SigninStep();
        }

        //[Test]
        public void Test_WebService()
        {
            Console.WriteLine(new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken());
            //new BaseSetup().DoSetUp();
        }

        //[Test]
        public void Get_LangWS()
        {
            string token = new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken();

            var client = new LanguageClient(token);
            //add

            Language a = new Language()
            {
                Name = "lang1",
                Level = "Beginner"
            };
            Language b = new Language()
            {
                Name = "lang2",
                Level = "Intermdediate"
            };
            client.AddData(a);
            client.AddData(b);


            var skills = client.ReadData();

            foreach (var skill in skills)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"#################{skill}");
                Console.WriteLine();
                client.DeleteData(skill.Id);
            }


            // delete

        }

        //[Test]
        public void Get_CertWS()
        {
            string token = new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken();

            var client = new CertificationClient(token);
            //add

            Certification a = new Certification()
            {
                Name = "certname1",
                Organisation = "org1",
                Year = "2020"
            };
            Certification b = new Certification()
            {
                Name = "certname2",
                Organisation = "org2",
                Year = "2022"
            };
            client.AddData(a);
            client.AddData(b);


            var skills = client.ReadData();

            foreach (var skill in skills)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"#################{skill}");
                Console.WriteLine();
                client.DeleteData(skill.Id);
            }


            // delete

        }


        //[Test]
        public void Get_SkillWS()
        {
            string token = new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken();

            var client = new SkillClient(token);
            //add

            Skill a = new Skill("AA", "Beginner");
            Skill b = new Skill("VV", "Beginner");

            client.AddData(a);
            client.AddData(b);


            var skills = client.ReadData();

            foreach (var skill in skills)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"#################{skill}");
                Console.WriteLine();
                client.DeleteData(skill.Id);
            }


            // delete

        }
        //[Test]
        public void Get_EducationWS()
        {
            string token = new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken();

            var client = new EducationClient(token);
            //add

            Education a = new Education()
            {
                DegreeTitle = "BCOM",
                DegreeName = "BACHELOR OF COMMERCE",
                Country = "New Zealand",
                InstituteName = "UoA",
                YearOfGraduation = "2020"
            };
            Education b = new Education()
            {
                DegreeTitle = "BCOM2",
                DegreeName = "BACHELOR OF COMMERCE2",
                Country = "New Zealand",
                InstituteName = "UoA",
                YearOfGraduation = "2021"
            };

            client.AddData(a);
            client.AddData(b);


            var skills = client.ReadData();

            foreach (var skill in skills)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"#################{skill}");
                Console.WriteLine();
                client.DeleteData(skill.Id);
            }


            // delete

        }


        //[Test]
        public void test_jsonconvert()
        {
            var x = new ShareSkill()
            {
                Active = ShareSkill.ActiveOption.Active,
                Title = "WEBSERVICE ADD",
                Description = "BB",
                Category = "Programming & Tech",
                SubCategory = "QA",
                //Tags = "a,b,c,",
                ServiceType = ShareSkill.ServiceTypeOption.HourlyBasisService,
                LocationType = ShareSkill.LocationTypeOption.Online,
                //SkillExchanges = "x,y,z",
                SkillExchangesList = new List<Tag>()
                {
                    new Tag()
                    {
                        Id = "AA",
                        Text = "AA"
                    },
                    new Tag()
                    {
                        Id = "BB",
                        Text = "BB"
                    }
                },
                TagsList = new List<Tag>()
                {
                    new Tag()
                    {
                        Id = "AA",
                        Text = "AA"
                    },
                    new Tag()
                    {
                        Id = "BB",
                        Text = "BB"
                    }
                },
                Availability = new Availability()
                {
                    StartDate = "2020-05-05",
                    EndDate = "2020-04-05",
                    DayEntries = new List<DayEntry>()
                    {
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = true
                        },
                        new DayEntry()
                        {
                            StartTime = "08:00AM",
                            EndTime = "04:00PM",
                            IsAvailable = false
                        }
                    }

                },
                Credit = 1.23M,
                SkillTrade = ShareSkill.SkillTradeOption.Credit
            };

            //Console.WriteLine(x.ToString());



            Console.WriteLine();

            string token = new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken();
            var client = new ShareSkillClient(token);

            IList<ShareSkill> list = client.ReadData(0, 999);

            foreach (var skill in list)
            {
                client.DeleteData(skill.Id);
                Console.WriteLine("___________________######");
                Console.WriteLine(skill.ToString());
                Console.WriteLine("____________________######");
            }


            //string id = client.AddData(x);

            //Console.WriteLine("ID IS " + id);
            //client.DeleteListingById("5ebcc3e684652600061dff1d");

        }

        //[Test]
        public void test_assembly()
        {
            Console.WriteLine(typeof(SkillClient).Assembly.DefinedTypes);

            foreach (var x in typeof(SkillClient).Assembly.DefinedTypes)
            {
                Console.WriteLine($"@@{x}");
            }
            foreach (var x in typeof(SkillClient).Assembly.GetTypes())
            {
                Console.WriteLine($"##{x}");
            }
            foreach (var x in typeof(SkillClient).Assembly.ExportedTypes)
            {
                Console.WriteLine($"$${x}");
            }
        }
    }
}
