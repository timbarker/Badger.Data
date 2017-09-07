namespace Badger.Data
{

    public interface ICommand
    {
        IDbCommand Build(IDbCommandBuilder builder);
    }
}
