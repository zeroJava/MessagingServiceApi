using MessageDbCore.DbRepositoryInterfaces;
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
    public class MessageDispatchRepositoryMsSql : IMessageDispatchRepository
    {
        private const string dispatchIdColumn = MessageDispatchColumn.ID;
        private const string dispatchAliasIdColumn = "md.ID";

        protected string connectionString;
        protected readonly IRepoTransaction repoTransaction;
        protected readonly bool transactionModeEnabled = false;
        public virtual string TableName { get; protected set; } = "messagedbo.MessageDispatchTable";

        public MessageDispatchRepositoryMsSql(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MessageDispatchRepositoryMsSql(string connectionString, IRepoTransaction repoTransaction)
        {
            this.connectionString = connectionString;
            this.repoTransaction = repoTransaction;
            this.transactionModeEnabled = true;
        }

        public List<MessageDispatch> GetAllDispatches()
        {
            try
            {
                string columns = GetSelectColumns(dispatchIdColumn);
                string query = string.Format("SELECT {0} FROM {1}", columns,
                    TableName);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query, null,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetAllDispatches", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        public MessageDispatch GetDispatchMatchingId(long dispatchId)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetdispatchMatchingIdQuery(dispatchId);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches.FirstOrDefault();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetdispatchMatchingId", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetdispatchMatchingIdQuery(long dispatchId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.ID, dispatchId)
            };
            string columns = GetSelectColumns(dispatchIdColumn);
            string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}",
                columns,
                TableName,
                MessageDispatchParameter.ID);

            return Tuple.Create(query, sqlParameters);
        }

        public List<MessageDispatch> GetDispatchesMatchingEmail(string email)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDispatchesMatchingEmailQuery(email);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetdispatchesMatchingId", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDispatchesMatchingEmailQuery(string email)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, email)
            };
            string columns = GetSelectColumns(dispatchIdColumn);
            string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2}",
                columns,
                TableName,
                MessageDispatchParameter.EMAIL_ADDRESS);

            return Tuple.Create(query, sqlParameters);
        }

        public List<MessageDispatch> GetDispatchesMatchingMessageId(long messageId)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDispatchesMatchingMessageIdQuery(messageId);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetdispatchesMatchingId", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDispatchesMatchingMessageIdQuery(long messageId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.MESSAGE_ID, messageId)
            };
            string columns = GetSelectColumns(dispatchIdColumn);
            string query = string.Format("SELECT {0} FROM {1} WHERE MESSAGEID = {2}",
                columns,
                TableName,
                MessageDispatchParameter.MESSAGE_ID);

            return Tuple.Create(query, sqlParameters);
        }

        public List<MessageDispatch> GetDispatchesNotReceivedMatchingEmail(string email)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDispatchesNotReceivedMatchingEmailQuery(email);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetDispatchesNotReceivedMatchingEmail", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDispatchesNotReceivedMatchingEmailQuery(string email)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, email)
            };
            string columns = GetSelectColumns(dispatchIdColumn);
            string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2} AND NOT MESSAGERECEIVED = 1",
                columns,
                TableName,
                MessageDispatchParameter.EMAIL_ADDRESS);

            return Tuple.Create(query, sqlParameters);
        }

        public List<MessageDispatch> GetDispatchesBetweenSenderReceiver(string senderEmailAddress,
            string receiverEmailAddress,
            long messageIdThreshold,
            int numberOfMessages)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDispathedBetweenSenderReceiverQuery(senderEmailAddress,
                    receiverEmailAddress,
                    messageIdThreshold);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    List<MessageDispatch> dispatches = new List<MessageDispatch>();
                    mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
                    return dispatches;
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "GetAlldispatchesBetweenSenderReceiver", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDispathedBetweenSenderReceiverQuery(string senderEmailAddress,
            string receiverEmailAddress,
            long messageIdThreshold)
        {
            string messageIdThresholdParameter = "@messageIdThreshold";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageParameter.SENDER_EMAIL_ADDRESS, senderEmailAddress),
                new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, receiverEmailAddress),
                new SqlParameter(messageIdThresholdParameter, messageIdThreshold),
            };
            string columns = GetSelectColumns(dispatchAliasIdColumn);
            string query = string.Format("SELECT {0} FROM {1} AS md " +
                "INNER JOIN messagedbo.MessageTable AS m ON m.ID = md.MESSAGEID " +
                "WHERE (m.SENDEREMAILADDRESS = {2} AND md.EMAILADDRESS = {3}) " +
                "OR (m.SENDEREMAILADDRESS = {3} AND md.EMAILADDRESS = {2}) " +
                "AND m.ID < {4}",
                columns,
                TableName,
                MessageParameter.SENDER_EMAIL_ADDRESS,
                MessageDispatchParameter.EMAIL_ADDRESS,
                messageIdThresholdParameter);

            return Tuple.Create(query, sqlParameters);
        }

        private MessageDispatch OnPopulateResultListCallBack(DbDataReader dbDataReader)
        {
            return Extractdispatch(dbDataReader);
        }

        private MessageDispatch Extractdispatch(DbDataReader dbDataReader)
        {
            MessageDispatch dispatch = new MessageDispatch();
            Populatedispatch(dispatch, dbDataReader);
            return dispatch;
        }

        private void Populatedispatch(MessageDispatch dispatch, DbDataReader dbDataReader)
        {
            if (dbDataReader[MessageDispatchColumn.ID] != null &&
                long.TryParse(dbDataReader[MessageDispatchColumn.ID].ToString(), out long id))
            {
                dispatch.Id = id;
            }
            if (dbDataReader[MessageDispatchColumn.EMAIL_ADDRESS] != null)
            {
                dispatch.EmailAddress = dbDataReader[MessageDispatchColumn.EMAIL_ADDRESS]
                    .ToString();
            }
            if (dbDataReader[MessageDispatchColumn.MESSAGE_ID] != null &&
                long.TryParse(dbDataReader[MessageDispatchColumn.MESSAGE_ID].ToString(), out long messageid))
            {
                dispatch.MessageId = messageid;
            }
            if (dbDataReader[MessageDispatchColumn.MESSAGE_RECEIVED] != null &&
                bool.TryParse(dbDataReader[MessageDispatchColumn.MESSAGE_RECEIVED].ToString(), out bool messageReceived))
            {
                dispatch.MessageReceived = messageReceived;
            }
            //var hh = dbDataReader[MessageDispatchColumn.MESSAGE_RECEIVED_TIME];
            if (dbDataReader[MessageDispatchColumn.MESSAGE_RECEIVED_TIME] != null &&
                DateTime.TryParse(dbDataReader[MessageDispatchColumn.MESSAGE_RECEIVED_TIME].ToString(), out DateTime receivedTime))
            {
                dispatch.MessageReceivedTime = receivedTime;
            }
        }

        public void InsertDispatch(MessageDispatch dispatch)
        {
            try
            {
                Tuple<string, SqlParameter[]> query = GetDispatchInsertQuery(dispatch);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQueryInsertCallback(dispatch, OnPopulateIdCallBack);
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "Insertdispatch", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDispatchInsertQuery(MessageDispatch dispatch)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, dispatch.EmailAddress),
                new SqlParameter(MessageDispatchParameter.MESSAGE_ID, dispatch.MessageId),
                new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED, GetDBValue(dispatch.MessageReceived)),
                new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED_TIME, GetDBValue(dispatch.MessageReceivedTime))
            };

            string insertStatement = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4})", TableName,
                MessageDispatchColumn.EMAIL_ADDRESS,
                MessageDispatchColumn.MESSAGE_ID,
                MessageDispatchColumn.MESSAGE_RECEIVED,
                MessageDispatchColumn.MESSAGE_RECEIVED_TIME);

            string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3})",
                MessageDispatchParameter.EMAIL_ADDRESS,
                MessageDispatchParameter.MESSAGE_ID,
                MessageDispatchParameter.MESSAGE_RECEIVED,
                MessageDispatchParameter.MESSAGE_RECEIVED_TIME);

            string query = string.Format("{0} {1}", insertStatement, valueSection);
            return Tuple.Create(query, sqlParameters);
        }

        private void OnPopulateIdCallBack(MessageDispatch dispatch, object result)
        {
            long id = Convert.ToInt64(result);
            dispatch.Id = id;
        }

        public void UpdateDispatch(MessageDispatch dispatch) // Tuple<string, IDbDataParameter[]> query where TParameter : IDbDataParameter
        {
            try
            {
                Tuple<string, SqlParameter[]> updatequery = GetDefaultDispatchUpdateQuery(dispatch);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(updatequery.Item1, updatequery.Item2,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "Updatedispatch", exception.Message);
                WriteErrorLog(message);
                throw;
            }
        }

        private Tuple<string, SqlParameter[]> GetDefaultDispatchUpdateQuery(MessageDispatch dispatch)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(MessageDispatchParameter.ID, dispatch.Id),
                new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, dispatch.EmailAddress),
                new SqlParameter(MessageDispatchParameter.MESSAGE_ID, dispatch.MessageId),
                new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED, GetDBValue(dispatch.MessageReceived)),
                new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED_TIME, GetDBValue(dispatch.MessageReceivedTime))
            };

            string updateTable = string.Format("UPDATE {0} SET", TableName);

            string setEmailAddress = string.Format("{0} = {1}",
                MessageDispatchColumn.EMAIL_ADDRESS,
                MessageDispatchParameter.EMAIL_ADDRESS);

            string setMessageId = string.Format("{0} = {1}",
                MessageDispatchColumn.MESSAGE_ID,
                MessageDispatchParameter.MESSAGE_ID);

            string setMessageReceived = string.Format("{0} = {1}",
                MessageDispatchColumn.MESSAGE_RECEIVED,
                MessageDispatchParameter.MESSAGE_RECEIVED);

            string setMessageReceivedTime = string.Format("{0} = {1}",
                MessageDispatchColumn.MESSAGE_RECEIVED_TIME,
                MessageDispatchParameter.MESSAGE_RECEIVED_TIME);

            string whereId = string.Format("WHERE {0} = {1}", MessageDispatchColumn.ID, MessageDispatchParameter.ID);

            string query = string.Format("{0} {1}, {2}, {3}, {4} {5}", updateTable,
                setEmailAddress,
                setMessageId,
                setMessageReceived,
                setMessageReceivedTime,
                whereId);

            return Tuple.Create(query, sqlParameters);
        }

        public void DeleteDispatch(MessageDispatch dispatch)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter(MessageDispatchParameter.ID, GetDBValue(dispatch.Id))
                };
                string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
                    MessageDispatchColumn.ID,
                    MessageDispatchParameter.ID);
                using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
                    connectionString))
                {
                    mssqlDbEngine.ExecuteQuery();
                }
            }
            catch (Exception exception)
            {
                string message = string.Format("Error encountered when executing {0} function in MessageDispatchRepositoryMsSql. Error\n{1}",
                    "Deletedispatch", exception.Message);
                WriteErrorLog(message);
                throw;
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

        public void Dispose()
        {
            //
        }

        protected static string GetSelectColumns(string id)
        {
            string columns = string.Format("{0}, {1}, {2}, {3}, {4}",
                id,
                MessageDispatchColumn.EMAIL_ADDRESS,
                MessageDispatchColumn.MESSAGE_ID,
                MessageDispatchColumn.MESSAGE_RECEIVED,
                MessageDispatchColumn.MESSAGE_RECEIVED_TIME);
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

        protected static object GetDBValue(object value)
        {
            return DbValueUtil.GetValidValue(value);
        }
    }
}