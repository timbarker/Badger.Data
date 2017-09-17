namespace Badger.Data
{
    public interface IRow
    {
        T Get<T>(string column);
    }
}
