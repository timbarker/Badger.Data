using System.Collections;
using System.Data.Common;
using System.Linq;

namespace Badger.Data.Commands
{
    sealed class DbCommandBuilder : DbBaseBuilder<IDbCommandBuilder>, IDbCommandBuilder
    {
        public DbCommandBuilder(DbCommand command)
            : base (command)
        {
        }
  
        public IDbExecutor Build()
        {
            return new DbCommandExecuter(this.command);
        }
    }
}
