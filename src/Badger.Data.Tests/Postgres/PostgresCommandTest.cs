using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresCommandTest : CommandTest<PostgresTestFixture>
    {
        public PostgresCommandTest(PostgresTestFixture fixture)
            : base (fixture)
        {            
        }
    }
}
