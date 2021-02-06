using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class RepoTransactionFactory
	{
		public static IRepoTransaction GetRepoTransaction(DatabaseEngineConstant databaseEngineOption, string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						return new RepoTransactionMsSql(connectionString);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the RepoTransactionFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}