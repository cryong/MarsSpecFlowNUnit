using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsFramework.WebDriver;

namespace MarsFramework.Factory
{
    public class PageFactory
    {
        public static object CreatePage(Type type, WebDriver.Driver driver)
        {
            if (HasDefaultConstructor(type))
            {
                return Activator.CreateInstance(type);
            }

            return Activator.CreateInstance(type, driver);
        }

        public static T CreatePage<T>(WebDriver.Driver driver) where T : class
        {
            return CreatePage(typeof(T), driver) as T;
        }

        private static bool HasDefaultConstructor(Type t)
        {
            return t.GetConstructor(Type.EmptyTypes) != null; // no need to check if it is a struct
        }
    }
}
