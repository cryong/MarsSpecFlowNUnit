using System.Collections.Generic;
using MarsFramework.WebService;
using MarsWebService.Model;

namespace MarsWebService.WebService
{
    public abstract class AbstractCRUDWSClient<T> : BaseClient, IWSClient<T> where T : IWSObject
    {
        public AbstractCRUDWSClient(string baseUrl, string token) : base(baseUrl, token)
        {
        }

        public abstract string AddData(T data);

        public abstract void DeleteData(string id);

        public abstract IList<T> ReadData();
    }
}
