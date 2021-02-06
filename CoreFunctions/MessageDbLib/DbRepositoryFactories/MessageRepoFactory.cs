using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using MessageDbLib.DbRepository.ADO;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class MessageRepoFactory
	{
		public static IMessageRepository GetMessageRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						return new MessageRepositoryMsSql(connectionString);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the MessageRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IMessageRepository GetMessageRepository(DatabaseEngineConstant databaseEngineOption,
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
							string message = "Wrong Repo transaction type is injected into MessageRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						return new MessageRepositoryMsSql(connectionString, repoTransactionMsSql);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the MessageRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}