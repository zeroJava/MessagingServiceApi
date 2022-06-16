using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository.ADO.MsSql;
using System;

namespace MessageDbLib.DbRepositoryFactories
{
	public static class OrganisationKeyRepoFactory
	{
		public static IOrganisationKeyRepository GetOrganisationKeyRepository(DatabaseEngineConstant databaseEngineOption,
			string connectionString)
		{
			switch (databaseEngineOption)
			{
				case DatabaseEngineConstant.MSSQLADODOTNET:
					{
						IOrganisationKeyRepository organisationKeyRepository =
							new OrganisationKeyRepository(connectionString);
						return organisationKeyRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the OrganisationRepositoryFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}

		public static IOrganisationKeyRepository GetOrganisationKeyRepository(DatabaseEngineConstant databaseEngineOption,
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
							string message = "Wrong Repo transaction type is injected into OrganisationKeyRepoFactory to be used with MSSQL.";
							throw new ApplicationException(message);
						}
						IOrganisationKeyRepository organisationKeyRepository =
							new OrganisationKeyRepository(connectionString, repoTransaction);
						return organisationKeyRepository;
					}
				default:
					{
						throw new InvalidOperationException("The option assigned to the OrganisationKeyRepoFactory does " +
							"not exist in the factories internal collection");
					}
			}
		}
	}
}