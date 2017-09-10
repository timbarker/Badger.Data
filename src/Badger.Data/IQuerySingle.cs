namespace Badger.Data
{
    public interface IQuerySingle<TResult> : IDbOperation<IDbQuerySingleBuilder<TResult>>
    {}
}
