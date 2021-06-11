namespace Moba.Domain.Core.Interfaces
{
    public interface IEntity
    {
        
    }
    public interface IEntity<T> :  IEntity
    {
        T Id { get; set; }
    }
}