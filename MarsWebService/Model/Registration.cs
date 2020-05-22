using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsWebService.Model
{
    public class Registration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public override string ToString()
        {
            return $"FirstName = '{FirstName}', LastName = '{LastName}', Email = '{Email}', Password = '{Password}', ConfirmPassword = '{Password}'";
        }
    }
}
