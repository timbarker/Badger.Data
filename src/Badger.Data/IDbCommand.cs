using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IDbCommand
    {
        Task<int> ExecuteAsync(CancellationToken cancellationToken);
        int Execute();
    }
}
