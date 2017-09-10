namespace Badger.Data
{

    public interface ICommand
    {
        IDbExecutor Build(IDbCommandBuilder builder);
    }
}
