using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Constants.TableConstants;
using MessageDbLib.DbEngine;
using MessageDbLib.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class MessageDispatchRepository : IMessageDispatchRepository
	{
		private const string dispatchIdColumn = MessageDispatchColumn.ID;
		private const string dispatchAliasIdColumn = "md.ID";

		protected string connectionString;
		protected readonly IRepoTransaction repoTransaction;
		protected readonly bool transactionModeEnabled = false;

		public virtual string TableName { get; protected set; } = "messagedbo.MessageDispatchTable";

		public MessageDispatchRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public MessageDispatchRepository(string connectionString, IRepoTransaction repoTransaction)
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
				string query = string.Format("SELECT {0} FROM {1}", columns, TableName);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query, null, connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetAllDispatches", exception);
			}
		}

		public MessageDispatch GetDispatchMatchingId(long dispatchId)
		{
			try
			{
				QueryBody queryBody = GetdispatchMatchingIdQuery(dispatchId);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchMatchingId", exception);
			}
		}

		private QueryBody GetdispatchMatchingIdQuery(long dispatchId)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(MessageDispatchParameter.ID, dispatchId)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}",
				columns,
				TableName,
				MessageDispatchParameter.ID);

			return new QueryBody(query, parameters);
		}

		public List<MessageDispatch> GetDispatchesMatchingEmail(string email)
		{
			try
			{
				QueryBody queryBody = GetDispatchesMatchingEmailQuery(email);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchMatchingEmail", exception);
			}
		}

		private QueryBody GetDispatchesMatchingEmailQuery(string email)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, email)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2}", columns,
				TableName,
				MessageDispatchParameter.EMAIL_ADDRESS);

			return new QueryBody(query, parameters);
		}

		public List<MessageDispatch> GetDispatchesMatchingMessageId(long messageId)
		{
			try
			{
				QueryBody queryBody = GetDispatchesMatchingMessageIdQuery(messageId);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchMatchingMessageId", exception);
			}
		}

		private QueryBody GetDispatchesMatchingMessageIdQuery(long messageId)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(MessageDispatchParameter.MESSAGE_ID, messageId)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE MESSAGEID = {2}", columns, TableName,
				MessageDispatchParameter.MESSAGE_ID);

			return new QueryBody(query, parameters);
		}

		public List<MessageDispatch> GetDispatchesNotReceivedMatchingEmail(string email)
		{
			try
			{
				QueryBody queryBody = GetDispatchesNotReceivedMatchingEmailQuery(email);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchesNotReceivedMatchingEmail", exception);
			}
		}

		private QueryBody GetDispatchesNotReceivedMatchingEmailQuery(string email)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, email)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2} AND NOT MESSAGERECEIVED = 1", columns,
				TableName,
				MessageDispatchParameter.EMAIL_ADDRESS);

			return new QueryBody(query, parameters);
		}

		public List<MessageDispatch> GetDispatchesBetweenSenderReceiver(string senderEmailAddress,
			string receiverEmailAddress,
			long messageIdThreshold,
			int numberOfMessages)
		{
			try
			{
				QueryBody queryBody = GetDispathedBetweenSenderReceiverQuery(senderEmailAddress, receiverEmailAddress,
					messageIdThreshold);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					List<MessageDispatch> dispatches = new List<MessageDispatch>();
					mssqlDbEngine.ExecuteReaderQuery(dispatches, OnPopulateResultListCallBack);
					return dispatches;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchesBetweenSenderReceiver", exception);
			}
		}

		private QueryBody GetDispathedBetweenSenderReceiverQuery(string senderEmailAddress,
			string receiverEmailAddress,
			long messageIdThreshold)
		{
			string messageIdThresholdParameter = "@messageIdThreshold";

			SqlParameter[] parameters = new SqlParameter[]
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

			return new QueryBody(query, parameters);
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
				QueryBody queryBody = GetDispatchInsertQuery(dispatch);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQueryInsertCallback(dispatch, OnPopulateIdCallBack);
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.InsertDispatch", exception);
			}
		}

		private QueryBody GetDispatchInsertQuery(MessageDispatch dispatch)
		{
			SqlParameter[] parameters = new SqlParameter[]
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

			return new QueryBody(query, parameters);
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
				QueryBody queryBody = GetDefaultDispatchUpdateQuery(dispatch);

				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(queryBody.Query, queryBody.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing MessageDispatchRepository.UpdateDispatch", exception);
			}
		}

		private QueryBody GetDefaultDispatchUpdateQuery(MessageDispatch dispatch)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(MessageDispatchParameter.ID, dispatch.Id),
				new SqlParameter(MessageDispatchParameter.EMAIL_ADDRESS, dispatch.EmailAddress),
				new SqlParameter(MessageDispatchParameter.MESSAGE_ID, dispatch.MessageId),
				new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED, GetDBValue(dispatch.MessageReceived)),
				new SqlParameter(MessageDispatchParameter.MESSAGE_RECEIVED_TIME, GetDBValue(dispatch.MessageReceivedTime))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setEmailAddress = string.Format("{0} = {1}", MessageDispatchColumn.EMAIL_ADDRESS,
				MessageDispatchParameter.EMAIL_ADDRESS);

			string setMessageId = string.Format("{0} = {1}", MessageDispatchColumn.MESSAGE_ID,
				MessageDispatchParameter.MESSAGE_ID);

			string setMessageReceived = string.Format("{0} = {1}", MessageDispatchColumn.MESSAGE_RECEIVED,
				MessageDispatchParameter.MESSAGE_RECEIVED);

			string setMessageReceivedTime = string.Format("{0} = {1}", MessageDispatchColumn.MESSAGE_RECEIVED_TIME,
				MessageDispatchParameter.MESSAGE_RECEIVED_TIME);

			string whereId = string.Format("WHERE {0} = {1}", MessageDispatchColumn.ID, MessageDispatchParameter.ID);

			string query = string.Format("{0} {1}, {2}, {3}, {4} {5}", updateTable,
				setEmailAddress,
				setMessageId,
				setMessageReceived,
				setMessageReceivedTime,
				whereId);

			return new QueryBody(query, parameters);
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
				throw new RepoDbException("Error while executing MessageDispatchRepository.DeleteDispatch", exception);
			}
		}

		public void Dispose()
		{
			//
		}

		protected static string GetSelectColumns(string id)
		{
			string columns = string.Format("{0}, {1}, {2}, {3}, {4}", id,
				MessageDispatchColumn.EMAIL_ADDRESS,
				MessageDispatchColumn.MESSAGE_ID,
				MessageDispatchColumn.MESSAGE_RECEIVED,
				MessageDispatchColumn.MESSAGE_RECEIVED_TIME);
			return columns;
		}

		protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
			string connectionString)
		{
			if (transactionModeEnabled && repoTransaction != null)
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