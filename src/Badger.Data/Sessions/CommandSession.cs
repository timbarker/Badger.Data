using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Badger.Data.Commands;

namespace Badger.Data.Sessions
{
    internal sealed class CommandSession : Session, ICommandSession
    {
        private readonly ParameterFactory _parameterFactory;

        public CommandSession(DbConnection connection, ParameterFactory parameterFactory, IsolationLevel isolationLevel)
            : base(connection, isolationLevel)
        {
            _parameterFactory = parameterFactory;
        }

        public int Execute(ICommand command)
        {
            var builder = new CommandBuilder(CreateCommand(), _parameterFactory);
            return command.Prepare(builder).Execute();
        }

        public async Task<int> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            var builder = new CommandBuilder(await CreateCommandAsync(cancellationToken), _parameterFactory);
            return await command.Prepare(builder).ExecuteAsync(cancellationToken).ConfigureAwait(false);
  
        }
    }
}
