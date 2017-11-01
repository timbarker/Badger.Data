using System.Data.Common;

namespace Badger.Data.Commands
{
    internal sealed class CommandBuilder : Builder<ICommandBuilder>, ICommandBuilder
    {
        public CommandBuilder(DbCommand command, ParameterFactory parameterFactory)
            : base (command, parameterFactory)
        {
        }
  
        public IPreparedCommand Build()
        {
            return new PreparedCommand(Command);
        }
    }
}
