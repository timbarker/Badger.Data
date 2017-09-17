using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IPreparedCommand
    {
        Task<int> ExecuteAsync(CancellationToken cancellationToken);
        int Execute();
    }
}
