using System.Collections.Generic;
using MarsWebService.Model;

namespace MarsWebService.WebService
{
    public interface IWSClient<T> where T : IWSObject
    {
        string AddData(T data);
        void DeleteData(string id);
        IList<T> ReadData();
    }
}