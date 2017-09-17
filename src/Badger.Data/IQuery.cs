namespace Badger.Data
{
    public interface IQuery<TResult> 
    {
        IPreparedQuery<TResult> Prepare(IQueryBuilder queryBuilder);
    }
}
