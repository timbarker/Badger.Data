using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IPreparedQuery<T> 
    {
        Task<T> ExecuteAsync(CancellationToken cancellationToken);
        T Execute();
    }
}
