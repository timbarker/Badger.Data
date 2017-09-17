namespace Badger.Data
{
    public interface IRow
    {
        T Get<T>(string column, T @default = default(T));
    }
}
