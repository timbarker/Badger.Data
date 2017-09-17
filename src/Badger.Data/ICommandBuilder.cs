namespace Badger.Data
{
    public interface ICommandBuilder
    {
        ICommandBuilder WithSql(string sql);
        ICommandBuilder WithParameter<T>(string name, T value);
        ICommandBuilder WithParameter(string name, string value, int length);
        IPreparedCommand Build();
    }
}
