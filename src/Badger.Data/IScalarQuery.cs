namespace Badger.Data
{

    public interface IScalarQuery<TResult> : IDbOperation<IDbScalarQueryBuilder<TResult>>
    {
    }
}
