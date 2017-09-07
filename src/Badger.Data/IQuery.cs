namespace Badger.Data
{
    public interface IQuery<TResult>
    {
        IDbQuery<TResult> Build(IDbQueryBuilder<TResult> builder);
    }
}
