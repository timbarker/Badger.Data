using Xunit;

namespace Badger.Data.Tests.Postgres
{
    public class PostgresQueryTest : QueryTest<PostgresTestFixture>
    {
        public PostgresQueryTest(PostgresTestFixture fixture) 
            : base(fixture)
        {            
        }
    }
}
