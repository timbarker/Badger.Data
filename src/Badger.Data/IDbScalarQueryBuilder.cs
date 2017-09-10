namespace Badger.Data
{
    public interface IDbScalarQueryBuilder<T> : IDbExecutorBuilder
    {
        IDbScalarQueryBuilder<T> WithSql(string sql);
        IDbScalarQueryBuilder<T> WithParameter<TParam>(string name, TParam value);
        IDbScalarQueryBuilder<T> WithDefault(T @default);
    }
}
