using System;
using System.ComponentModel;
using System.Linq;

namespace MarsFramework.Utilities
{
    public class EnumConverter
    {
        private EnumConverter() { }
        public static T GetValueFromDescription<T>(Type enumType, string description)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }
            foreach (var field in enumType.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }
            throw new ArgumentException($"Unable to derive Enum using {nameof(description)} '{description}' for type '{enumType.Name}'");
        }

        public static string GetDescription<T>(T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                foreach (int val in Enum.GetValues(type))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    if (memInfo[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), false)
                        .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }
            throw new ArgumentException("Incorrect argument provided. Either argument is not Enum or description does not exist");
        }
    }
}
