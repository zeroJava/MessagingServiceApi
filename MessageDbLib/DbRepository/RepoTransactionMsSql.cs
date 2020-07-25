using MessageDbCore.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MessageDbLib.DbRepository
{
    public class RepoTransactionMsSql : IRepoTransaction
    {
        protected readonly string connectionString;
        protected readonly SqlConnection sqlConnection;
        protected SqlTransaction sqlTransaction;

        public bool TransactionInvoked { get; protected set; }

        public IDbConnection DbConnection
        {
            get
            {
                return sqlConnection;
            }
        }

        public IDbTransaction DbTransaction
        {
            get
            {
                return sqlTransaction;
            }
        }

        public RepoTransactionMsSql(string connectionString)
        {
            this.connectionString = connectionString;
            this.sqlConnection = new SqlConnection(connectionString);
        }

        public void BeginTransaction()
        {
            if (sqlConnection == null)
            {
                return;
            }
            OpenConnection(sqlConnection);
            sqlTransaction = sqlConnection.BeginTransaction();
            TransactionInvoked = true;
        }

        protected void OpenConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            else if (sqlConnection.State == ConnectionState.Broken)
            {
                sqlConnection.Close();
                sqlConnection.Open();
            }
        }

        public void Callback()
        {
            try
            {
                if (sqlTransaction == null)
                {
                    return;
                }
                sqlTransaction.Rollback();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void Callback(string transactionName)
        {
            try
            {
                if (sqlTransaction == null)
                {
                    return;
                }
                sqlTransaction.Rollback(transactionName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public void Commit()
        {
            if (sqlTransaction == null)
            {
                return;
            }
            sqlTransaction.Commit();
        }

        public void Dispose()
        {
            try
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }
    }
}