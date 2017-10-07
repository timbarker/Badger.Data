using System;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface ICommandSession : IDisposable
    {
        int Execute(ICommand command);
        Task<int> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default);
        void Commit();
    }
}
