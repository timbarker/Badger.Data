namespace Badger.Data
{
    public interface IScalarQuery<TResult>
    {
        IDbScalarQuery<TResult> Build(IDbScalarQueryBuilder<TResult> builder);
    }
}
