using Xunit;

namespace Badger.Data.Tests.SqlServer
{
    [Trait("ExcludeFromTravis", "True")]
    public class SqlServerQueryTest : QueryTest<SqlServerTestFixture>
    {
        public SqlServerQueryTest(SqlServerTestFixture fixture) 
            : base(fixture)
        {
        }
    }
}
