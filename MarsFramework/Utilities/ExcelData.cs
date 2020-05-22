using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsFramework.Utilities
{
    public class ExcelData
    {
        //key
        public string Key { get; set; }
        // tuple that stores col and value

        // maybe better as dictionary
        public IList<ValueTuple<string, string>> DataList { get; } = new List<ValueTuple<string, string>>();

        public string FetchColumnValue(string columnName)
        {
            // ignore casing
            return DataList.Where(pair => pair.Item1.ToUpper() == columnName.ToUpper()).Select(pair => pair.Item2).SingleOrDefault();
        }
    }
}
