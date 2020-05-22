using System;

namespace MarsFramework.Model.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CustomJSONConverterAttribute : Attribute
    {
        public CustomJSONConverterAttribute()
        {
        }
    }
}
