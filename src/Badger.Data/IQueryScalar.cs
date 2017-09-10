namespace Badger.Data
{

    public interface IQueryScalar<TResult> : IDbOperation<IDbQueryScalarBuilder<TResult>>
    {
    }
}
