namespace Badger.Data
{
    public interface IAsyncTransactionSession : IAsyncSession
    {
        void Commit();
    }
}
