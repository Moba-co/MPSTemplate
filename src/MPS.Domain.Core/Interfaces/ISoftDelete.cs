namespace MPS.Domain.Core.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}