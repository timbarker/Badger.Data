using Xunit;

namespace Badger.Data.Tests.SqlServer
{
    [Trait("Category", "Integration")]
    public class SqlServerCommandTest : CommandTest<SqlServerTestFixture>
    {
        public SqlServerCommandTest(SqlServerTestFixture fixture)
            : base(fixture)
        {
        }
    }
}
