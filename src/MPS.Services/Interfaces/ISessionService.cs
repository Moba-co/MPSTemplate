using System.Threading;
using System.Threading.Tasks;

namespace MPS.Services.Interfaces
{
    public interface ISessionService
    {
        bool Set(string key, string value);
        string Get(string key);
        bool Change(string key, string newValue);
        bool KeyExist(string key);
        bool Delete(string key);
        Task CommitAsync(CancellationToken cancellationToken);
        Task LoadAsync(CancellationToken cancellationToken);
        bool Clear();
    }
}
