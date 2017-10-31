using Xunit;

namespace Badger.Data.Tests.Postgres
{
    [Trait("Category", "Travis")]
    public class PostgresCommandTest : CommandTest<PostgresTestFixture>
    {
        public PostgresCommandTest(PostgresTestFixture fixture)
            : base (fixture)
        {            
        }
    }
}
