using System;
using System.Data;

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