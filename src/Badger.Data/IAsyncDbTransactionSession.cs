namespace Badger.Data
{
    public interface IAsyncDbTransactionSession : IAsyncDbSession
    {
        void Commit();
    }
}
