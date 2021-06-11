using Moba.Domain.Core.Interfaces;

namespace Moba.Domain.Core
{
    public abstract class BaseEntity<T> : IEntity<T>
    { 
        public T Id { get; set; }
    }
}