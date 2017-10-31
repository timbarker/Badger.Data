using Xunit;

namespace Badger.Data.Tests.Sqlite
{
    [Trait("Travis", "True")]
    public class SqliteCommandTest : CommandTest<SqliteTestFixture>
    {
        public SqliteCommandTest(SqliteTestFixture fixture)
            : base (fixture)
        {
        }
    }
}
