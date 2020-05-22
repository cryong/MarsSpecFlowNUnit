using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MarsFramework.Model.Attributes;
using MarsFramework.Utilities;
using Newtonsoft.Json;

namespace MarsFramework.Factory
{
    public class ObjectFactory
    {
        //https://stackoverflow.com/questions/30662183/how-to-create-a-dynamic-ienumerable
        private ObjectFactory() { }

        // can only handle value types or types that implement IConvertible 
        public static T CreateInstance<T>(ExcelData data) where T : class, new()
        {
            // Load Excel based on the type
            var type = typeof(T);
            var instance = Activator.CreateInstance<T>();

            foreach (var propertyInfo in GetProperties())
            {
                // if my custom attribute is found
                CustomJSONConverterAttribute attribute = (CustomJSONConverterAttribute)propertyInfo.GetCustomAttributes(false).
                                                            FirstOrDefault(a => a.GetType() == typeof(CustomJSONConverterAttribute));

                if (attribute != null)
                {
                    // convert JSON formatted string to object
                    var propertyType = propertyInfo.PropertyType;
                    var convertedObj = JsonConvert.DeserializeObject(GetColumnValue(propertyInfo.Name), propertyType);
                    propertyInfo.SetValue(instance, convertedObj, null);
                }
                else
                {
                    var convertedValue = ConvertToPropertyValue(propertyInfo.PropertyType, GetColumnValue(propertyInfo.Name)) ?? default;
                    propertyInfo.SetValue(instance, convertedValue, null);
                }
            }
            return instance;

            IEnumerable<PropertyInfo> GetProperties()
            {
                foreach (var property in type.GetProperties().Where(p => p.CanWrite))
                {
                    yield return property;
                }
            }
            string GetColumnValue(string columnName)
            {
                return data.FetchColumnValue(columnName);
            }
        }

        private static object ConvertToPropertyValue(Type propertyType, string value)
        {
            if (value == null)
            {
                return default;
            }
            if (propertyType.IsEnum)
            {
                return EnumConverter.GetValueFromDescription<IConvertible>(propertyType, value);
            }
            // as long as it's not a reference type, this should work
            //return value == null ? default: propertyType.IsEnum? EnumConverter.GetValueFromDescription<IConvertible>(propertyType, value) : Convert.ChangeType(value, propertyType);
            return Convert.ChangeType(value, propertyType);
        }
    }
}
