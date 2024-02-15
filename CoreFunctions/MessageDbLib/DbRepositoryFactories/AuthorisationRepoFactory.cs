using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository.ADO.MsSql;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class AuthorisationRepoFactory
	{
		public static IAuthorisationRepository GetAuthorisationRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MsSqlAdoDotNet:
					{
						IAuthorisationRepository authorisationRepository =
							new DbRepository.ADO.MsSql.AuthorisationRepository(connectionString);
						return authorisationRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the AuthorisationRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IAuthorisationRepository GetAuthorisationRepository(DatabaseEngineConstant databaseEngineOption,
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
							string message = "Wrong Repo transaction type is injected into AuthorisationRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						IAuthorisationRepository authorisationRepository =
							new DbRepository.ADO.MsSql.AuthorisationRepository(connectionString,
							repoTransaction);
						return authorisationRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the AuthorisationRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}