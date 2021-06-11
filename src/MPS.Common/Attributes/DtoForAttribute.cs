using System;

namespace MPS.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DtoForAttribute : Attribute
    {
        public DtoForAttribute(Type entityClass)
        {
            EntityClass = entityClass;
        }

        public DtoForAttribute()
        {

        }
        public Type EntityClass { get; set; }
        
    }
}