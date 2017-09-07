namespace Badger.Data
{
    public  interface IDbTransactionSession : IDbSession
    {
        void Commit();
    }
}
