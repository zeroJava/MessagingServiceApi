using MessageDbCore.DatabaseEngines;
using MessageDbCore.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MessageDbLib.DbEngine
{
    public class MssqlDbEngine : DbSqlEngine
    {
        protected readonly SqlParameter[] mssqlParameters;
        //protected readonly IRepoTransaction repoTransaction;
        private readonly bool transationModeEnabled = false;

        private SqlConnection sqlConnection;
        private SqlTransaction sqlTransaction;

        // Move the sql connector into a adapter and trasnaction, and do a despendeny injection

        // We may have to remove the sqlconnection from using statement, and make into a variable. 

        public MssqlDbEngine(string sqlQuery, SqlParameter[] mssqlParameters, string connectionString)
        {
            this.SqlQuery = sqlQuery;
            base.ConnectionString = connectionString;
            this.mssqlParameters = mssqlParameters ?? (new SqlParameter[0]);
            this.sqlConnection = new SqlConnection(connectionString);
        }

        public MssqlDbEngine(string sqlQuery, SqlParameter[] mssqlParameters, string connectionString,
            IRepoTransaction repoTransaction)
        {
            this.SqlQuery = sqlQuery;
            base.ConnectionString = connectionString;
            this.mssqlParameters = mssqlParameters ?? (new SqlParameter[0]);
            //this.repoTransaction = repoTransaction;
            this.transationModeEnabled = true;
            InitialiseSqlConnection(repoTransaction);
        }

        protected virtual void InitialiseSqlConnection(IRepoTransaction repoTransaction)
        {
            if (repoTransaction == null)
            {
                return;
            }
            sqlConnection = repoTransaction.DbConnection as SqlConnection;
            sqlTransaction = repoTransaction.DbTransaction as SqlTransaction;
        }

        public override void Dispose()
        {
            if (transationModeEnabled ||
                sqlConnection == null)
            {
                return;
            }
            try
            {
                CloseConnection(sqlConnection);
                sqlConnection.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public override void ExecuteQuery()
        {
            if (mssqlParameters == null ||
                mssqlParameters.Count() == 0)
            {
                throw new ApplicationException("No paramater were assigned");
            }
            ExecuteSqlQuery(ConnectionString, SqlQuery, mssqlParameters);
        }

        private void ExecuteSqlQuery(string connectionString, string sqlQuery,
            SqlParameter[] mssqlParameters)
        {
            /*using (SqlConnection sqlConnection = GetSqlConnection(connectionString))
            {
                ExecuteSqlCommand(sqlQuery, sqlConnection, mssqlParameters);
            }*/
            ExecuteSqlCommand(sqlQuery, sqlConnection, mssqlParameters);
        }

        /*protected SqlConnection GetSqlConnection(string connectionString)
        {
            if (transationModeEnabled)
            {
                if (this.sqlConnection == null)
                {
                    throw new ApplicationException("Invalid SQL connection type is injected into MssqlDbEngine.");
                }
                return this.sqlConnection;
            }
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            return sqlConnection;
        }*/

        private void ExecuteSqlCommand(string sqlQuery, SqlConnection sqlConnection,
            SqlParameter[] mssqlParameters)
        {
            using (SqlCommand sqlCommand = GetSqlCommand(sqlQuery, sqlConnection))
            {
                sqlCommand.Parameters.AddRange(mssqlParameters);
                OpenConnection(sqlConnection);
                sqlCommand.ExecuteNonQuery();
                CloseConnection(sqlConnection);
            }
        }

        protected virtual SqlCommand GetSqlCommand(string sqlQuery, SqlConnection sqlConnection)
        {
            if (transationModeEnabled)
            {
                if (sqlTransaction == null)
                {
                    throw new ApplicationException("Invalid SQL transaction type is injected into MssqlDbEngine.");
                }
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection,
                    sqlTransaction);
                return sqlCommand;
            }
            SqlCommand sqlCommand2 = new SqlCommand(sqlQuery, sqlConnection);
            return sqlCommand2;
        }

        protected void OpenConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            else if (sqlConnection.State == System.Data.ConnectionState.Broken)
            {
                CloseConnection(sqlConnection);
                sqlConnection.Open();
            }
        }

        protected void CloseConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection == null ||
                transationModeEnabled)
            {
                return;
            }
            sqlConnection.Close();
        }

        public override void ExecuteQueryInsertCallback<TEntity>(TEntity entity, Action<TEntity, object> populateIdCallBack)
        {
            if (mssqlParameters == null)
            {
                throw new ApplicationException("No paramater were assigned");
            }
            InsertSqlQueryWithCallback(ConnectionString, SqlQuery,
                mssqlParameters,
                entity,
                populateIdCallBack);
        }

        private void InsertSqlQueryWithCallback<TEntity>(string connectionString, string sqlQuery,
            SqlParameter[] mssqlParameters,
            TEntity entity,
            Action<TEntity, object> populateIdCallBack)
        {
            /*using (SqlConnection sqlConnection = GetSqlConnection(connectionString))
            {
                InsertSqlCommandWithCallback(sqlQuery, sqlConnection,
                    mssqlParameters,
                    entity,
                    populateIdCallBack);
            }*/
            InsertSqlCommandWithCallback(sqlQuery, sqlConnection,
                mssqlParameters,
                entity,
                populateIdCallBack);
        }

        private void InsertSqlCommandWithCallback<TEntity>(string sqlQuery, SqlConnection sqlConnection,
            SqlParameter[] mssqlParameters,
            TEntity entity,
            Action<TEntity, object> populateIdCallBack)
        {
            using (SqlCommand sqlCommand = GetSqlCommand(sqlQuery, sqlConnection))
            {
                sqlCommand.Parameters.AddRange(mssqlParameters);
                OpenConnection(sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlCommand.CommandText = "SELECT @@IDENTITY";
                if (populateIdCallBack != null)
                {
                    populateIdCallBack.Invoke(entity, sqlCommand.ExecuteScalar());
                }
                CloseConnection(sqlConnection);
            }
        }

        public override void ExecuteReaderQuery<TEntity>(List<TEntity> entities, PopulateResultListCallBack<TEntity> populateResultListCallBack)
        {
            if (mssqlParameters == null)
            {
                throw new ApplicationException("No paramater were assigned");
            }
            ReaderQueryWithCallback(ConnectionString, SqlQuery,
                mssqlParameters,
                entities,
                populateResultListCallBack);
        }

        private void ReaderQueryWithCallback<TEntity>(string connectionString, string sqlQuery,
            SqlParameter[] mssqlParameters,
            List<TEntity> entities,
            PopulateResultListCallBack<TEntity> populateResultListCallBack)
        {
            /*using (SqlConnection sqlConnection = GetSqlConnection(connectionString))
            {
                ReaderSqlCommandWithCallback(sqlQuery, sqlConnection,
                    mssqlParameters,
                    entities,
                    populateResultListCallBack);
            }*/
            ReaderSqlCommandWithCallback(sqlQuery, sqlConnection,
                mssqlParameters,
                entities,
                populateResultListCallBack);
        }

        private void ReaderSqlCommandWithCallback<TEntity>(string sqlQuery, SqlConnection sqlConnection,
            SqlParameter[] mssqlParameters,
            List<TEntity> entities,
            PopulateResultListCallBack<TEntity> populateResultListCallBack)
        {
            using (SqlCommand sqlCommand = GetSqlCommand(sqlQuery, sqlConnection))
            {
                sqlCommand.Parameters.AddRange(mssqlParameters);
                OpenConnection(sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    if (populateResultListCallBack != null)
                    {
                        TEntity entityObject = populateResultListCallBack.Invoke(sqlDataReader);
                        entities.Add(entityObject);
                    }
                }
                CloseConnection(sqlConnection);
            }
        }
    }
}