using Xunit;

namespace Badger.Data.Tests.Postgres
{
    [Trait("Category", "Integration")]
    public class PostgresCommandTest : CommandTest<PostgresTestFixture>
    {
        public PostgresCommandTest(PostgresTestFixture fixture)
            : base(fixture)
        {
        }
    }
}
