namespace Badger.Data
{
    public interface ICommand
    {
        int Execute(IDbCommandBuilder builder);
    }
}
