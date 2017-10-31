using Xunit;

namespace Badger.Data.Tests.Sqlite
{
    [Trait("Travis", "True")]
    public class SqliteQueryTest : QueryTest<SqliteTestFixture>
    {
        public SqliteQueryTest(SqliteTestFixture fixture)
            : base(fixture)
        {
        }
    }
}
