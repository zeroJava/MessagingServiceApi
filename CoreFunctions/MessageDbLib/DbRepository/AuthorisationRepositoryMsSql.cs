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
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.DbRepository.ADO
{
    public class AuthorisationRepositoryMsSql : IAuthorisationRepository
    {
        protected readonly string connectionString;
        protected readonly IRepoTransaction repoTransaction;
        protected readonly bool transactionModeEnabled = false;
        public virtual string TableName { get; protected set; } = "dbo.AuthorisationTable";

        public AuthorisationRepositoryMsSql(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public AuthorisationRepositoryMsSql(string connectionString, IRepoTransaction repoTransaction)
        {
            this.connectionString = connectionString;
            this.repoTransaction = repoTransaction;
            this.transactionModeEnabled = true;
        }

        public Authorisation GetAuthorisationMatchingAuthCode(Guid guid)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetAuthorisationMatchingAuthCodeQuery(guid);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<Authorisation> authorisations = new List<Authorisation>();
                    mssqlDbEngine.ExecuteReaderQuery(authorisations, OnPopulateResultListCallBack);
                    return authorisations.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AuthorsationRepositoryMsSql. Error\n{1}",
                    "Get-Authorisation-Matching-Auth-Code", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetAuthorisationMatchingAuthCodeQuery(Guid guid)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.AUTHORISATION_CODE, guid),
            };

            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1} WHERE AUTHORISATIONCODE = {2}", columns,
                TableName,
                AuthorisationParameter.AUTHORISATION_CODE);

            return Tuple.Create(query, parameters);
        }

        public Authorisation GetAuthorisationMatchingId(long id)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetAuthorisationMatchingIdQuery(id);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<Authorisation> authorisations = new List<Authorisation>();
                    mssqlDbEngine.ExecuteReaderQuery(authorisations, OnPopulateResultListCallBack);
                    return authorisations.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AuthorsationRepositoryMsSql. Error\n{1}",
                    "Get-Authorisation-Matching-Id", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetAuthorisationMatchingIdQuery(long id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.ID, id),
            };

            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns,
                TableName,
                AuthorisationParameter.ID);

            return Tuple.Create(query, parameters);
        }

        public List<Authorisation> GetAuthorisationsGreaterThanEndTime(DateTime endtime)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetAuthorisationGreaterThanEndTimeQuery(endtime);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<Authorisation> authorisations = new List<Authorisation>();
                    mssqlDbEngine.ExecuteReaderQuery(authorisations, OnPopulateResultListCallBack);
                    return authorisations;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AuthorsationRepositoryMsSql. Error\n{1}",
                    "Get-Authorisation-Greater-Than-EndTime", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetAuthorisationGreaterThanEndTimeQuery(DateTime dateTime)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.END_TIME, dateTime),
            };

            string columns = GetSelectColumns();
            string query = string.Format("SELECT {0} FROM {1} WHERE ENDTIME >= {2}", columns,
                TableName,
                AuthorisationParameter.END_TIME);

            return Tuple.Create(query, parameters);
        }

        public void InsertAuthorisation(Authorisation authorisation)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetInsertAuthorisationQuery(authorisation);
                using (DbSqlEngine sqlEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    sqlEngine.ExecuteQueryInsertCallback(authorisation, OnPopulatedIdCallback);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in AthorisationRepositoryMsSql. Error\n{1}",
                    "Insert-Authorisation", exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetInsertAuthorisationQuery(Authorisation authorisation)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.AUTHORISATION_CODE, GetDBValue(authorisation.AuthorisationCode)),
                new SqlParameter(AuthorisationParameter.START_TIME, GetDBValue(authorisation.StartTime)),
                new SqlParameter(AuthorisationParameter.END_TIME, GetDBValue(authorisation.EndTime)),
                new SqlParameter(AuthorisationParameter.USER_ID, GetDBValue(authorisation.UserId)),
            };

            string insertSection = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4})", TableName,
                AuthorisationColumn.AUTHORISATION_CODE,
                AuthorisationColumn.START_TIME,
                AuthorisationColumn.END_TIME,
                AuthorisationColumn.USER_ID);

            string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3})", AuthorisationParameter.AUTHORISATION_CODE,
                AuthorisationParameter.START_TIME,
                AuthorisationParameter.END_TIME,
                AuthorisationParameter.USER_ID);

            string query = string.Format("{0} {1}", insertSection, valueSection);
            return Tuple.Create(query, parameters);
        }

        private void OnPopulatedIdCallback(Authorisation authorisation, object result)
        {
            long id = Convert.ToInt64(result);
            authorisation.Id = id;
        }

        public void UpdateAuthorisation(Authorisation authorisation)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetUpdateAuthorisationQuery(authorisation);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} in AuthorisationRepositoryMsSql. Error\n{1}",
                    "Update-Authorisation",
                    exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetUpdateAuthorisationQuery(Authorisation authorisation)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.ID, GetDBValue(authorisation.Id)),
                new SqlParameter(AuthorisationParameter.AUTHORISATION_CODE, GetDBValue(authorisation.AuthorisationCode)),
                new SqlParameter(AuthorisationParameter.START_TIME, GetDBValue(authorisation.StartTime)),
                new SqlParameter(AuthorisationParameter.END_TIME, GetDBValue(authorisation.EndTime)),
                new SqlParameter(AuthorisationParameter.USER_ID, GetDBValue(authorisation.UserId)),
            };

            string updateStr = string.Format("UPDATE {0} SET", TableName);

            string setAuthCode = string.Format("{0} = {1}", AuthorisationColumn.AUTHORISATION_CODE, AuthorisationParameter.AUTHORISATION_CODE);
            string setStartTime = string.Format("{0} = {1}", AuthorisationColumn.START_TIME, AuthorisationParameter.START_TIME);
            string setEndTime = string.Format("{0} = {1}", AuthorisationColumn.END_TIME, AuthorisationParameter.END_TIME);
            string setUserId = string.Format("{0} = {1}", AuthorisationColumn.USER_ID, AuthorisationParameter.USER_ID);

            string whereId = string.Format("WHERE {0} = {1}", AuthorisationColumn.ID, GetDBValue(authorisation.Id));

            string query = string.Format("{0} {1}, {2}, {3}, {4} {5}", updateStr, setAuthCode,
                setStartTime,
                setEndTime,
                setUserId,
                whereId);

            return Tuple.Create(query, parameters);
        }

        public void DeleteAuthorisation(Authorisation authorisation)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDeletionAuthorisationQuery(authorisation);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} in AuthorisationRepositoryMsSql. Error\n{1}",
                    "Delete-Authorisation",
                    exception.ToString());
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDeletionAuthorisationQuery(Authorisation authorisation)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(AuthorisationParameter.ID, GetDBValue(authorisation.Id)),
            };

            string query = string.Format("DELETE FROM {0} WHERE ID = {1}", TableName,
                AuthorisationParameter.ID);

            return Tuple.Create(query, parameters);
        }

        public void Dispose()
        {
            //
        }

        protected static string GetSelectColumns()
        {
            string columns = string.Format("{0}, {1}, {2}, {3}, {4}",
                AuthorisationColumn.ID,
                AuthorisationColumn.AUTHORISATION_CODE,
                AuthorisationColumn.START_TIME,
                AuthorisationColumn.END_TIME,
                AuthorisationColumn.USER_ID);
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

        private Authorisation OnPopulateResultListCallBack(DbDataReader dataReader)
        {
            Authorisation authorisation = new Authorisation();
            PopulateAuthorisation(authorisation, dataReader);
            return authorisation;
        }

        private void PopulateAuthorisation(Authorisation authorisation, DbDataReader dataReader)
        {
            string idColumnStr = dataReader[AuthorisationColumn.ID] != null ?
                dataReader[AuthorisationColumn.ID].ToString() :
                string.Empty;

            if (!string.IsNullOrEmpty(idColumnStr) &&
                long.TryParse(idColumnStr, out long id))
            {
                authorisation.Id = id;
            }

            Guid guidResult;
            string authorisationColumnStr = dataReader[AuthorisationColumn.AUTHORISATION_CODE] != null ?
                dataReader[AuthorisationColumn.AUTHORISATION_CODE].ToString() :
                string.Empty;

            if (!string.IsNullOrEmpty(authorisationColumnStr) &&
                Guid.TryParse(authorisationColumnStr, out guidResult))
            {
                authorisation.AuthorisationCode = guidResult;
            }

            DateTime starttime;
            string starttimeStr = dataReader[AuthorisationColumn.START_TIME] != null ?
                dataReader[AuthorisationColumn.START_TIME].ToString() :
                string.Empty;

            if (!string.IsNullOrEmpty(starttimeStr) &&
                DateTime.TryParse(starttimeStr, out starttime))
            {
                authorisation.StartTime = starttime;
            }

            DateTime endtime;
            string endtimeStr = dataReader[AuthorisationColumn.END_TIME] != null ?
                dataReader[AuthorisationColumn.END_TIME].ToString() :
                string.Empty;

            if (!string.IsNullOrEmpty(endtimeStr) &&
                DateTime.TryParse(endtimeStr, out endtime))
            {
                authorisation.EndTime = endtime;
            }

            long userId;
            string userIdStr = dataReader[AuthorisationColumn.USER_ID] != null ?
                dataReader[AuthorisationColumn.USER_ID].ToString() :
                string.Empty;

            if (!string.IsNullOrEmpty(userIdStr) &&
                long.TryParse(userIdStr, out userId))
            {
                authorisation.UserId = userId;
            }
        }

        protected void WriteErrorLog(string errorMessage)
        {
            LogFile.WriteErrorLog(errorMessage);
        }

        protected void WriteInfoLog(string infoMessage)
        {
            LogFile.WriteInfoLog(infoMessage);
        }

        protected object GetDBValue(object value)
        {
            return DbValueUtil.GetValidValue(value);
        }
    }
}