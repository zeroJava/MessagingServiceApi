using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository.ADO.MsSql;
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
				case DatabaseEngineConstant.MsSqlAdoDotNet:
					{
						return new MessageRepository(connectionString);
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
				case DatabaseEngineConstant.MsSqlAdoDotNet:
					{
						RepoTransaction repoTransactionMsSql = repoTransaction as RepoTransaction;
						if (repoTransactionMsSql == null)
						{
							string message = "Wrong Repo transaction type is injected into MessageRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						return new MessageRepository(connectionString, repoTransactionMsSql);
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