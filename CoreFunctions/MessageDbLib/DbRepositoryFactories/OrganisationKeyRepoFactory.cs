using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
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
                        IOrganisationKeyRepository organisationKeyRepository = new DbRepository.ADO.OrganisationKeyRepositoryMsSql(connectionString);
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
                        RepoTransactionMsSql repoTransactionMsSql = repoTransaction as RepoTransactionMsSql;
                        if (repoTransactionMsSql == null)
                        {
                            string message = "Wrong Repo transaction type is injected into OrganisationKeyRepoFactory to be used with MSSQL.";
                            throw new ApplicationException(message);
                        }
                        IOrganisationKeyRepository organisationKeyRepository = new DbRepository.ADO.OrganisationKeyRepositoryMsSql(connectionString,
                            repoTransaction);
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