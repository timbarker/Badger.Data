using System.Collections;
using System.Data.Common;
using System.Linq;

namespace Badger.Data
{
    sealed class DbCommandBuilder : DbBaseBuilder<IDbCommandBuilder>, IDbCommandBuilder
    {
        public DbCommandBuilder(DbCommand command)
            : base (command)
        {
        }
  
        public IDbCommand Build()
        {
            return new DbCommandExecuter(this.command);
        }
    }
}
