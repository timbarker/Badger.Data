using Xunit;

namespace Badger.Data.Tests.Postgres;

[Trait("Category", "Integration")]
public class PostgresCommandTest(PostgresTestFixture fixture) : CommandTest<PostgresTestFixture>(fixture)
{
}
