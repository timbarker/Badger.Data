using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface ISession : IDisposable
    {
        int ExecuteCommand(ICommand command);
        TResult ExecuteQuery<TResult>(IQuery<TResult> query);
    }
}
