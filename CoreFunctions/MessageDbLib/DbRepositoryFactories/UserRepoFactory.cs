using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository;
using MessageDbLib.DbRepository.ADO;
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
                        return new UserRepositoryMsSql(connectionString);
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
                        RepoTransactionMsSql repoTransactionMsSql = repoTransaction as RepoTransactionMsSql;
                        if (repoTransactionMsSql == null)
                        {
                            string message = "Wrong Repo transaction type is injected into UserRepoFactory to be used with MSSQL.";
                            throw new ApplicationException(message);
                        }
                        return new UserRepositoryMsSql(connectionString, repoTransactionMsSql);
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