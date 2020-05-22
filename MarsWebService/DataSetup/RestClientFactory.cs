using System;
using System.Collections.Generic;
using MarsWebService.Model;
using MarsWebService.WebService;

namespace MarsCommonFramework.DataSetup
{
    public class RestClientFactory
    {
        private static readonly Dictionary<Type, Func<string, object>> _factoryByType = new Dictionary<Type, Func<string, object>> {
            { typeof(Language),   (t) => new LanguageClient(t) },
            { typeof(Skill), (t) => new SkillClient(t) },
            { typeof(Education), (t) => new EducationClient(t) },
            { typeof(Certification), (t) => new CertificationClient(t) },
            { typeof(ShareSkill), (t) => new ShareSkillClient(t) }};

        public static IWSClient<T> Create<T>(string token) where T : IWSObject
        {
            if (_factoryByType.TryGetValue(typeof(T), out Func<string, object> concreteFactory))
            {
                return (IWSClient<T>)concreteFactory(token);
            }

            throw new ArgumentException($"Unable to determine which client to instantiate for type '{typeof(T)}'");
        }

        public static object Create(object dto, string token)
        {
            if (_factoryByType.TryGetValue(dto.GetType(), out Func<string, object> concreteFactory))
            {
                return concreteFactory(token);
            }
            throw new ArgumentException($"Unable to determine which client to instantiate for type '{dto.GetType()}'");
        }

    }
}
