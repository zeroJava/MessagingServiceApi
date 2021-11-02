using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using MessageDbLib.DbRepository.ADO.MsSql;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class UserRepoFactory
	{
		public static IUserRepository GetUserRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						return new UserRepository(connectionString);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the UserRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IUserRepository GetUserRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString,
			IRepoTransaction repoTransaction)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						RepoTransaction repoTransactionMsSql = repoTransaction as RepoTransaction;
						if (repoTransactionMsSql == null)
						{
							string message = "Wrong Repo transaction type is injected into UserRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						return new UserRepository(connectionString, repoTransactionMsSql);
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the UserRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}