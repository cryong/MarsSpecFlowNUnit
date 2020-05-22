using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsWebService.WebService;

namespace MarsWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            //new BaseSetup().DoSetUp();
            Console.WriteLine(new AuthenticationClient("changhoon.ryong@gmail.com", "testm3").GetToken());
        }
    }
}
