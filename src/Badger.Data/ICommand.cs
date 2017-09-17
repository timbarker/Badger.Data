namespace Badger.Data
{
    public interface ICommand
    {
        IPreparedCommand Prepare(ICommandBuilder builder);
    }
}
