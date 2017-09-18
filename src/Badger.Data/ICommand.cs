namespace Badger.Data
{
    /// <summary>
    /// A command that can be executed.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Prepars the command so that it can be executed.
        /// </summary>
        /// <param name="commandBuilder">a builder to help create the prepared command.</param>
        IPreparedCommand Prepare(ICommandBuilder commandBuilder);
    }
}
