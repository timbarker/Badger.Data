using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badger.Data
{
    public interface IDbSession : IDisposable
    {
        int ExecuteCommand(ICommand command);
        IEnumerable<TResult> ExecuteQuery<TResult>(IQuery<TResult> query);
        TResult ExecuteQuery<TResult>(IQueryScalar<TResult> query);
    }
}
