namespace Badger.Data
{
    public interface IDbRow
    {
        T Get<T>(string column);
    }
}
