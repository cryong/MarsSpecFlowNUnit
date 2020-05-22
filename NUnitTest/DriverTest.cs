using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarsFramework.WebDriver;
using MarsFramework.Service;
using MarsFramework.Utilities;
using NUnit.Framework;

namespace NUnitTest
{
    //[TestFixture]
    public class DriverTest
    {
        //[Test]
        public void Test_Driver()
        {



            //DriverManager.StartDriver(Browser.Chrome);
            //Driver init = DriverManager.GetDriver();

            //Driver driver1 = DriverManager.GetDriver();
            //Console.WriteLine(driver1 == init);
            //Driver driver2 = DriverManager.GetDriver();
            //Console.WriteLine(driver2 == init);
            //Driver driver3 = DriverManager.GetDriver();
            //Console.WriteLine(driver3 == init);


            Thread t1 = new Thread(() =>
            {
                Read("Login", "TestUser");
                //DriverManager.StartDriver(Browser.Chrome);
                //Driver driver = DriverManager.GetDriver();
                //Console.WriteLine(driver.ToString());
                //Thread.Sleep(5000);
                //Console.WriteLine($"what 1 - {ExcelDataReaderUtil.FetchRowUsingKey("TestUser")}");
            });

            Thread t2 = new Thread(() =>
            {
                Read("Registration", "DuplicateEmail");
                //DriverManager.StartDriver(Browser.Chrome);
                //Driver driver = DriverManager.GetDriver();
                //Console.WriteLine(driver.ToString());
                //Thread.Sleep(1000);
                //Console.WriteLine($"what 2 - 2 - {ExcelDataReaderUtil.FetchRowUsingKey("DuplicateEmail")}");
            });


            ////parameterized thread
            //Thread t3 = new Thread(p =>
            //{

            //});

            t1.Start();
            t2.Start();
            ////passing parameter to parameterized thread
            //t3.Start();

            ////wait for t1 to fimish
            t1.Join();

            ////wait for t2 to finish
            t2.Join();

            ////wait for t3 to finish
            //t3.Join();
        }

        private void StartDriver()
        {
            DriverManager.StartDriver(Browser.Chrome);
            Driver driver = DriverManager.GetDriver();
            Console.WriteLine(driver.ToString());
        }


        private void Read(string sheet, string key)
        {
            try
            {
                ExcelDataReaderUtil.LoadWorsheet("C:\\Users\\changhoon\\source\\repos\\MarsAdvancedTask\\SpecFlowTest\\TestData\\Mars.xlsx", sheet);
                Console.WriteLine($"Reading from [{sheet}] using key [{key}] - {ExcelDataReaderUtil.FetchRowUsingKey(key)}");
                Console.WriteLine($"{ExcelDataReaderUtil.FetchRowUsingKey(key).DataList}");
                foreach (ValueTuple<string, string> vt in ExcelDataReaderUtil.FetchRowUsingKey(key).DataList)
                {
                    Console.WriteLine($"{vt.Item1} - {vt.Item2}");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine($"wtf failed {sheet} - {key}");
                Console.WriteLine(e);
            }


        }
    }
}
