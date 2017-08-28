namespace Badger.Data
{
    public interface IQuery<TResult>
    {
        TResult Execute(IDbQueryBuilder builder);
    }
}
