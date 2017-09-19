using System.Data;
using System.Data.Common;
using Badger.Data.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data.Sessions
{
    internal sealed class CommandSession : Session, ICommandSession
    {
        public CommandSession(DbConnection connection, IsolationLevel isolationLevel)
            : base(connection, isolationLevel)
        {
        }

        public int Execute(ICommand command)
        {
            var builder = new CommandBuilder(CreateCommand());
            return command.Prepare(builder).Execute();
        }

        public async Task<int> ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            var builder = new CommandBuilder(await CreateCommandAsync(cancellationToken));
            return await command.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
  
        }

        public Task<int> ExecuteAsync(ICommand command)
        {
            return ExecuteAsync(command, CancellationToken.None);
        }
    }
}
