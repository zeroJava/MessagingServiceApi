using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
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
                case DatabaseEngineConstant.MSSQLADODOTNET:
                    {
                        IAuthorisationRepository authorisationRepository = new DbRepository.ADO.AuthorisationRepositoryMsSql(connectionString);
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
                case DatabaseEngineConstant.MSSQLADODOTNET:
                    {
                        RepoTransactionMsSql repoTransactionMsSql = repoTransaction as RepoTransactionMsSql;
                        if (repoTransactionMsSql == null)
                        {
                            string message = "Wrong Repo transaction type is injected into AuthorisationRepoFactory to be used with MSSQL.";
                            throw new ApplicationException(message);
                        }
                        IAuthorisationRepository authorisationRepository = new DbRepository.ADO.AuthorisationRepositoryMsSql(connectionString,
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