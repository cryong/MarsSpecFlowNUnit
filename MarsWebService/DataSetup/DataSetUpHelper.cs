using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MarsWebService.Model;
using MarsWebService.WebService;

namespace MarsCommonFramework.DataSetup
{
    public class DataSetUpHelper
    {
        private readonly string _username;
        private readonly string _password;
        private string _token;
        private string Token
        {
            get
            {
                if (_token == null)
                {
                    _token = new AuthenticationClient(_username, _password).GetToken();
                }
                return _token;
            }
        }

        public DataSetUpHelper(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public string GetOrAdd<T>(T obj) where T : IWSObject
        {
            var crudClient = RestClientFactory.Create<T>(Token);
            IList<T> list = crudClient.ReadData();
            if (list != null && list.Count > 0)
            {
                var matchingObj = list.Where(o => o.Equals(obj)).FirstOrDefault();
                if (matchingObj != null)
                {
                    // probably better off setting it here TBH
                    return matchingObj.Id;
                }
            }
            return crudClient.AddData(obj);
        }

        public string GetOrAdd(object obj)
        {
            if (!typeof(IWSObject).IsAssignableFrom(obj.GetType()))
            {
                throw new ArgumentException($"Invalid argument '{nameof(obj)}' {obj}");
            }

            object crudClient = RestClientFactory.Create(obj, Token);
            MethodInfo readMethod = crudClient.GetType().GetMethod("ReadData");
            IEnumerable list = (IEnumerable)readMethod.Invoke(crudClient, null);

            foreach (var o in list)
            {
                if (o.Equals(obj))
                {
                    return ((IWSObject)o).Id;
                }
            }

            MethodInfo addMethod = crudClient.GetType().GetMethod("AddData");
            return (string)addMethod.Invoke(crudClient, new[] { obj });
        }


        public void Delete<T>(T obj) where T : IWSObject
        {
            var crudClient = RestClientFactory.Create<T>(Token);
            crudClient.DeleteData(obj.Id);
        }

        public void Delete(object obj)
        {
            if (!typeof(IWSObject).IsAssignableFrom(obj.GetType()))
            {
                throw new ArgumentException($"Invalid argument '{nameof(obj)}' {obj}");
            }

            dynamic crudClient = RestClientFactory.Create(obj, Token); // dirty way to avoid using reflection
            crudClient.DeleteData(((IWSObject)obj).Id);
        }
    }
}
