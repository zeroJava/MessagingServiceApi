using MessageDbCore.EntityClasses;
using MessageDbCore.Repositories;
using MessageDbLib.Constants.TableConstants;
using MessageDbLib.DbEngine;
using MessageDbLib.Logging;
using MessageDbLib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MessageDbLib.DbRepository.ADO
{
    public class AccessRepositoryMsSql : IAccessRepository
    {
        protected readonly string connectionString;
        protected readonly IRepoTransaction repoTransaction;
        protected readonly bool transactionModeEnabled = false;
        public virtual string TableName { get; protected set; } = "dbo.AccessTable";

        public AccessRepositoryMsSql(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public AccessRepositoryMsSql(string connectionString, IRepoTransaction repoTransaction)
        {
            this.connectionString = connectionString;
            this.repoTransaction = repoTransaction;
            this.transactionModeEnabled = true;
        }

        public Access GetAccessMatchingToken(string token)
        {
            try
            {
                QueryBody query = GetAccessMatchingTokenQuery(token);
                using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Query, query.Parameters,
                    connectionString))
                {
                    List<Access> accesses = new List<Access>();
                    dbEngine.ExecuteReaderQuery(accesses, OnPopulateResultListCallBack);
                    return accesses.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AccessRepositoryMsSql. Error\n{1}",
                    "Get-Access-Matching-Token", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private QueryBody GetAccessMatchingTokenQuery(string token)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AccessParameter.TOKEN, token)
            };

            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1} WHERE TOKEN = {2}", columns,
                TableName,
                AccessParameter.TOKEN);

            return new QueryBody(query, parameters);
        }

        public Access GetAccessMatchingId(long id)
        {
            try
            {
                QueryBody query = GetAccessMatchingIdQuery(id);
                using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Query, query.Parameters,
                    connectionString))
                {
                    List<Access> accesses = new List<Access>();
                    dbEngine.ExecuteReaderQuery(accesses, OnPopulateResultListCallBack);
                    return accesses.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AccessRepositoryMsSql. Error\n{1}",
                    "Get-Access-Matching-Id", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private QueryBody GetAccessMatchingIdQuery(long id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AccessParameter.ID, id)
            };

            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns,
                TableName,
                AccessParameter.ID);

            return new QueryBody(query, parameters);
        }

        private Access OnPopulateResultListCallBack(DbDataReader dataReader)
        {
            Access access = new Access();
            PopulateAccess(access, dataReader);
            return access;
        }

        private void PopulateAccess(Access access, DbDataReader dataReader)
        {
            string idColumn = dataReader[AccessColumn.ID] != null ? dataReader[AccessColumn.ID].ToString() :
                string.Empty;

            long id;
            if (!string.IsNullOrEmpty(idColumn) &&
                long.TryParse(idColumn, out id))
            {
                access.Id = id;
            }

            if (dataReader[AccessColumn.ORGANISATION] != null)
            {
                access.Organisation = dataReader[AccessColumn.ORGANISATION].ToString();
            }

            if (dataReader[AccessColumn.TOKEN] != null)
            {
                access.Token = dataReader[AccessColumn.TOKEN].ToString();
            }

            string useridColumn = dataReader[AccessColumn.USER_ID] != null ? dataReader[AccessColumn.USER_ID].ToString() : string.Empty;

            long userid;
            if (!string.IsNullOrEmpty(useridColumn) &&
                long.TryParse(useridColumn, out userid))
            {
                access.UserId = userid;
            }

            string starttimeColumn = dataReader[AccessColumn.START_TIME] != null ? dataReader[AccessColumn.START_TIME].ToString() :
                string.Empty;

            DateTime starttime;
            if (!string.IsNullOrEmpty(starttimeColumn) &&
                DateTime.TryParse(starttimeColumn, out starttime))
            {
                access.StartTime = starttime;
            }

            string endtimeColumn = dataReader[AccessColumn.END_TIME] != null ? dataReader[AccessColumn.END_TIME].ToString() :
                string.Empty;

            DateTime endtime;
            if (!string.IsNullOrEmpty(endtimeColumn) &&
                DateTime.TryParse(endtimeColumn, out endtime))
            {
                access.EndTime = endtime;
            }

            string scopeColumn = dataReader[AccessColumn.SCOPE] != null ? dataReader[AccessColumn.SCOPE].ToString() :
                string.Empty;

            string[] scope = new string[0];
            if (!string.IsNullOrEmpty(scopeColumn))
            {
                string[] list = scopeColumn.Split(';');
                if (list.Length > 0)
                {
                    scope = list;
                }
            }
            access.Scope = scope;
        }

        public void InsertAccess(Access access)
        {
            try
            {
                QueryBody query = GetInsertAccessQuery(access);
                using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Query, query.Parameters,
                    connectionString))
                {
                    dbEngine.ExecuteQueryInsertCallback(access, OnPopulateIdCallBack);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AccessRepositoryMsSql. Error\n{1}",
                    "Inser-Access", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private QueryBody GetInsertAccessQuery(Access access)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AccessParameter.ORGANISATION, GetDBValue(access.Organisation)),
                new SqlParameter(AccessParameter.TOKEN, GetDBValue(access.Token)),
                new SqlParameter(AccessParameter.USER_ID, GetDBValue(access.UserId)),
                new SqlParameter(AccessParameter.START_TIME, GetDBValue(access.StartTime)),
                new SqlParameter(AccessParameter.END_TIME, GetDBValue(access.EndTime)),
                new SqlParameter(AccessParameter.SCOPE, GetDBValue(JoinScopeList(access.Scope))),
            };

            string insert = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6})", TableName,
                AccessColumn.ORGANISATION,
                AccessColumn.TOKEN,
                AccessColumn.USER_ID,
                AccessColumn.START_TIME,
                AccessColumn.END_TIME,
                AccessColumn.SCOPE);

            string values = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5})", AccessParameter.ORGANISATION,
                AccessParameter.TOKEN,
                AccessParameter.USER_ID,
                AccessParameter.START_TIME,
                AccessParameter.END_TIME,
                AccessParameter.SCOPE);

            string stringquery = string.Format("{0} {1}", insert, values);

            return new QueryBody(stringquery, parameters);
        }

        private string JoinScopeList(string[] scope)
        {
            if (scope == null ||
                scope.Length == 0)
            {
                return null;
            }

            string joinString = string.Join(";", scope);
            return joinString;
        }

        private void OnPopulateIdCallBack(Access access, object result)
        {
            long id = Convert.ToInt64(result);
            access.Id = id;
        }

        public void UpdateAccess(Access access)
        {
            try
            {
                QueryBody query = GetUpdateAccessQuery(access);
                using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Query, query.Parameters,
                    connectionString))
                {
                    dbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AccessRepositoryMsSql. Error\n{1}",
                    "Update-Access", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private QueryBody GetUpdateAccessQuery(Access access)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AccessParameter.ID, GetDBValue(access.Id)),
                new SqlParameter(AccessParameter.ORGANISATION, GetDBValue(access.Organisation)),
                new SqlParameter(AccessParameter.TOKEN, GetDBValue(access.Token)),
                new SqlParameter(AccessParameter.USER_ID, GetDBValue(access.UserId)),
                new SqlParameter(AccessParameter.START_TIME, GetDBValue(access.StartTime)),
                new SqlParameter(AccessParameter.END_TIME, GetDBValue(access.EndTime)),
                new SqlParameter(AccessParameter.SCOPE, GetDBValue(JoinScopeList(access.Scope))),
            };

            string updateTable = string.Format("UPDATE {0} SET", TableName);

            string setOrganisation = string.Format("{0} = {1}", AccessColumn.ORGANISATION,
                AccessParameter.ORGANISATION);

            string setToken = string.Format("{0} = {1}", AccessColumn.TOKEN,
                AccessParameter.TOKEN);

            string setUserId = string.Format("{0} = {1}", AccessColumn.USER_ID,
                AccessParameter.USER_ID);

            string setStartTime = string.Format("{0} = {1}", AccessColumn.START_TIME,
                AccessParameter.START_TIME);

            string setEndTime = string.Format("{0} = {1}", AccessColumn.END_TIME,
                AccessParameter.END_TIME);

            string setScope = string.Format("{0} = {1}", AccessColumn.SCOPE,
                AccessParameter.SCOPE);

            string whereId = string.Format("WHERE {0} = {1}", AccessColumn.ID,
                AccessParameter.ID);

            string query = string.Format("{0} {1}, {2}, {3}, {4}, {5}, {6} {7}", updateTable,
                setOrganisation,
                setToken,
                setUserId,
                setStartTime,
                setEndTime,
                setScope,
                whereId);

            return new QueryBody(query, parameters);
        }

        public void DeleteAccess(Access access)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter(AccessParameter.ID, GetDBValue(access.Id))
                };

                string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
                    AccessColumn.ID,
                    AccessParameter.ID);

                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string eMessage = string.Format("Error encountered when executing {0} function in AccessRepositoryMsSql. Error\n{1}",
                    "Delete-Access", exception.Message);
                WriteErrorLog(eMessage);
                throw;
            }
        }

        protected static string GetSelectColumns()
        {
            string columns = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                AccessColumn.ID,
                AccessColumn.ORGANISATION,
                AccessColumn.TOKEN,
                AccessColumn.USER_ID,
                AccessColumn.START_TIME,
                AccessColumn.END_TIME,
                AccessColumn.SCOPE);
            return columns;
        }

        protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
            string connectionString)
        {
            if (transactionModeEnabled &&
                repoTransaction != null)
            {
                MssqlDbEngine transactionMssqlEngine = new MssqlDbEngine(query, mssqlParameters,
                    connectionString,
                    repoTransaction);
                return transactionMssqlEngine;
            }
            MssqlDbEngine mssqlDbEngine = new MssqlDbEngine(query, mssqlParameters,
                connectionString);
            return mssqlDbEngine;
        }

        protected void WriteErrorLog(string errorMessage)
        {
            LogFile.WriteErrorLog(errorMessage);
        }

        protected object GetDBValue(object value)
        {
            return DbValueUtil.GetValidValue(value);
        }
    }
}