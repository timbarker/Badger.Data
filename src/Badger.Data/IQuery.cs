namespace Badger.Data
{
    /// <summary>
    /// A query that can be executed.
    /// </summary>
    public interface IQuery<TResult> 
    {
        /// <summary>
        /// Prepars the query so that it can be executed.
        /// </summary>
        /// <param name="queryBuilder">a builder to help create the prepared query.</param>
        IPreparedQuery<TResult> Prepare(IQueryBuilder queryBuilder);
    }
}
