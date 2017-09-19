using System.Collections;
using System.Data.Common;
using System.Linq;

namespace Badger.Data.Commands
{
    internal sealed class CommandBuilder : Builder<ICommandBuilder>, ICommandBuilder
    {
        public CommandBuilder(DbCommand command)
            : base (command)
        {
        }
  
        public IPreparedCommand Build()
        {
            return new PreparedCommand(this.command);
        }
    }
}
