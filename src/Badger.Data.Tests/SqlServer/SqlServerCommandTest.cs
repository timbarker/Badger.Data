using Xunit;

namespace Badger.Data.Tests.SqlServer
{
    [Trait("ExcludeFromTravis", "True")]
    public class SqlServerCommandTest : CommandTest<SqlServerTestFixture>
    {
        public SqlServerCommandTest(SqlServerTestFixture fixture) 
            : base(fixture)
        {
        }
    }
}
