using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class MessageDispatchRepoFactory
	{
		public static IMessageDispatchRepository GetDispatchRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						return new DbRepository.ADO.MessageDispatchRepositoryMsSql(connectionString);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the MessageDispatchRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IMessageDispatchRepository GetDispatchRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString,
			IRepoTransaction repoTransaction)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						RepoTransactionMsSql repoTransactionMsSql = repoTransaction as RepoTransactionMsSql;
						if (repoTransactionMsSql == null)
						{
							string message = "Wrong Repo transaction type is injected into MessageDispatchRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						return new DbRepository.ADO.MessageDispatchRepositoryMsSql(connectionString, repoTransactionMsSql);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the MessageDispatchRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}