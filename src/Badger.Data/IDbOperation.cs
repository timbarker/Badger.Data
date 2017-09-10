namespace Badger.Data
{
    public interface IDbOperation<TBuilder>
        where TBuilder : IDbExecutorBuilder
    {
        IDbExecutor Prepare(TBuilder builder);
    }
}
