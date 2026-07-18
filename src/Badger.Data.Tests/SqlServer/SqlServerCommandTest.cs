using Xunit;

namespace Badger.Data.Tests.SqlServer;

[Trait("Category", "Integration")]
public class SqlServerCommandTest(SqlServerTestFixture fixture) : CommandTest<SqlServerTestFixture>(fixture)
{
}
