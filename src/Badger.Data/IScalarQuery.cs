namespace Badger.Data
{
    public interface IScalarQuery<TResult>
    {
        IDbExecutor Build(IDbScalarQueryBuilder<TResult> builder);
    }
}
