namespace Badger.Data.Tests.Queries
{
    internal class NullScalarQuery : IQuery<string>
    {
        public IPreparedQuery<string> Prepare(IQueryBuilder builder)
        {
            return builder
                .WithSql("select null")
                .WithScalar<string>()
                .Build();
        }
    }
}
