using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ExcelDataReader;

namespace MarsFramework.Utilities
{
    public class ExcelDataReaderUtil
    {

        private static ThreadLocal<DataTable> workSheetDataLocal = new ThreadLocal<DataTable>();
        //private static DataTable workSheetData;
        //private static readonly IList<ExcelData> dataWithKeys = new List<ExcelData>();
        private static readonly ThreadLocal<List<ExcelData>> dataWithKeysLocal = new ThreadLocal<List<ExcelData>>(() => new List<ExcelData>());


        //private static object lockObject = new object();
        private ExcelDataReaderUtil() { }

        public static void LoadWorsheet(string filePath, string workSheetName)
        {
            if (workSheetDataLocal.Value != null && workSheetDataLocal.Value.TableName == workSheetName)
            {
                // don't load the same thing again
                return;
            }

            dataWithKeysLocal.Value?.Clear();
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) // FileShare.Read for accessing same file by concurrently
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    // getting specific worksheet
                    workSheetDataLocal.Value = result.Tables[workSheetName];
                    
                }
                //Thread.Sleep(3000);
                //Console.WriteLine("finished sleeping");
            }

            PopulateData();
        }

        private static void PopulateData()
        {
            // headers are already skipped
            for (int i = 0; i < workSheetDataLocal.Value.Rows.Count; i++)
            {
                var excelData = new ExcelData()
                {
                    Key = workSheetDataLocal.Value.Rows[i]["key"].ToString()
                };

                // loop through columns and populate data list

                for (int j = 0; j < workSheetDataLocal.Value.Columns.Count; j++)
                {
                    if (workSheetDataLocal.Value.Columns[j].ColumnName == "key")
                    {
                        continue;
                    }

                    var data = ValueTuple.Create(workSheetDataLocal.Value.Columns[j].ColumnName, workSheetDataLocal.Value.Rows[i][j].ToString());
                    excelData.DataList.Add(data);
                    //Console.WriteLine($"Adding data {workSheetData.Columns[j].ColumnName} - {workSheetData.Rows[i][j]}");
                }

                dataWithKeysLocal.Value.Add(excelData);
            }
        }

        // support regex and return a list?
        public static ExcelData FetchRowUsingKey(string key)
        {
            // returns the first match
            if (workSheetDataLocal.Value == null)
            {
                throw new ExcelDataReaderException($"Unable to retrieve row using {nameof(key)} : '{key}' because {nameof(workSheetDataLocal.Value)} is null");
            }
            try
            {
                return (from data in dataWithKeysLocal.Value
                        where (data.Key == key)
                        select data).SingleOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return default;
            }
        }

        //public static IList<ExcelData> FetchRowsUsingRegex(string regexKey)
        //{
        //    IList<ExcelData> matchingDataList = new List<ExcelData>();
        //    foreach (var data in dataWithKeys)
        //    {
        //        if (Regex.Match(data.Key, regexKey, RegexOptions.IgnoreCase).Success)
        //        {
        //            matchingDataList.Add(data);
        //            // not sure if ignore case is needed
        //        }

        //    }
        //    return matchingDataList;
        //}

        //public static IEnumerator<ExcelData> FetchRow()
        //{
        //    if (workSheetDataLocal.Value == null)
        //    {
        //        throw new ExcelDataReaderException($"Unable to retrieve row because {nameof(workSheetDataLocal.Value)} is null");
        //    }
        //    foreach (var data in dataWithKeysLocal.Value)
        //    {
        //        yield return data;
        //    }
        //}
    }

    public class ExcelDataReaderException : Exception
    {
        public ExcelDataReaderException() { }
        public ExcelDataReaderException(string message) : base(message) { }
        public ExcelDataReaderException(string message, Exception inner) : base(message, inner) { }

    }
}