using System;

namespace MPS.Common.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MapFromAttribute : Attribute
    {
        public MapFromAttribute()
        {

        }

        public MapFromAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
        public string PropertyName { get; set; }

    }
}