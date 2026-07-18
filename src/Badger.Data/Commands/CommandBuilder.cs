using System.Data.Common;

namespace Badger.Data.Commands;

internal sealed class CommandBuilder(DbCommand command, ParameterFactory parameterFactory) : Builder<ICommandBuilder>(command, parameterFactory), ICommandBuilder
{
    public IPreparedCommand Build()
    {
        return new PreparedCommand(Command);
    }
}
