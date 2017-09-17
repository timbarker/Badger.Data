namespace Badger.Data
{
    public  interface ITransactionSession : ISession
    {
        void Commit();
    }
}
