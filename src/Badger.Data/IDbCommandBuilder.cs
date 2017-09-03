namespace Badger.Data
{
    public interface IDbCommandBuilder
    {
        IDbCommandBuilder WithSql(string sql);
        IDbCommandBuilder WithParameter<T>(string name, T value);
        IDbCommandBuilder WithParameter(string name, string value, int length);
        int Execute();
    }
}
