namespace Badger.Data
{
    public interface IDbQueryScalarBuilder<T> : IDbExecutorBuilder
    {
        IDbQueryScalarBuilder<T> WithSql(string sql);
        IDbQueryScalarBuilder<T> WithParameter<TParam>(string name, TParam value);
        IDbQueryScalarBuilder<T> WithDefault(T @default);
    }
}
