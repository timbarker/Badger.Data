namespace Badger.Data
{
    public interface IQuery<TResult>
    {
        IDbExecutor Build(IDbQueryBuilder<TResult> builder);
    }
}
