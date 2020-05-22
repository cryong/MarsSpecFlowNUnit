using System;

namespace MarsFramework.Model.Attributes
{
    #region custom attribute
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class CombineColumnsAttribute : Attribute
    {
        readonly string columnName;

        public CombineColumnsAttribute(string columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentException("Mandatory parameter msising from attribute");
            }

            this.columnName = columnName;
        }

        public string ColumnName => columnName;

        public string Suffixes { get; set; }
        public string Delimiter { get; set; }
    }

#endregion
}
