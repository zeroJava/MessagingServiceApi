using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class AccessRepoFactory
	{
		public static IAccessRepository GetAuthorisationRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						IAccessRepository accessRepository = new DbRepository.ADO.AccessRepositoryMsSql(connectionString);
						return accessRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the AccessRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IAccessRepository GetAuthorisationRepository(DatabaseEngineConstant databaseEngineOption,
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
							string message = "Wrong Repo transaction type is injected into AccessRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						IAccessRepository authorisationRepository = new DbRepository.ADO.AccessRepositoryMsSql(connectionString,
							repoTransaction);
						return authorisationRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the AccessRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}