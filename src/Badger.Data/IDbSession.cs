using System;

namespace Badger.Data
{
    public interface IDbSession : IDisposable
    {
        int ExecuteCommand(ICommand command);
        TResult ExecuteQuery<TResult>(IQuery<TResult> query);
    }
}
