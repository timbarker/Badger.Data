namespace Badger.Data
{
    public interface IQuery<TResult> : IDbOperation<IDbQueryBuilder<TResult>>
    {
    }
}
