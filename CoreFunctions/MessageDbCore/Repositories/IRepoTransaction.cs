using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbCore.Repositories
{
    public interface IRepoTransaction : IDisposable
    {
        bool TransactionInvoked { get; }
        IDbConnection DbConnection { get; }
        IDbTransaction DbTransaction { get; }

        void BeginTransaction();
        void Commit();
        void Callback();
        void Callback(string transactionName);
    }
}