using MPS.Domain.Core.Interfaces;

namespace MPS.Domain.Core
{
    public abstract class BaseEntity<T> : IEntity<T>
    { 
        public T Id { get; set; }
    }
}