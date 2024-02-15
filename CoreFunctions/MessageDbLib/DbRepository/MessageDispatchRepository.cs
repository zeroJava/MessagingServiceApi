using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Constants.TableConstants;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Column = MessageDbLib.Constants.TableConstants.MessageDispatchColumn;
using Prmetr = MessageDbLib.Constants.TableConstants.MessageDispatchParameter;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class MessageDispatchRepository : BaseRepository, IMessageDispatchRepository
	{
		private const string dispatchIdColumn = Column.ID;
		private const string dispatchAliasIdColumn = "md.ID";

		//protected string connectionString;
		//protected readonly IRepoTransaction repoTransaction;
		//protected readonly bool transactionModeEnabled = false;

		public virtual string TableName { get; protected set; } = "messagedbo.MessageDispatchTable";

		public MessageDispatchRepository(string connectionString) :
			base(connectionString)
		{
			//this.connectionString = connectionString;
		}

		public MessageDispatchRepository(string connectionString,
			IRepoTransaction repoTransaction) : base(connectionString,
				repoTransaction)
		{
			//this.connectionString = connectionString;
			//this.repoTransaction = repoTransaction;
			//this.transactionModeEnabled = true;
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
				new SqlParameter(Prmetr.ID, dispatchId)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}",
				columns,
				TableName,
				Prmetr.ID);

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
				new SqlParameter(Prmetr.EMAIL_ADDRESS, email)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2}", columns,
				TableName,
				Prmetr.EMAIL_ADDRESS);

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
				new SqlParameter(Prmetr.MESSAGE_ID, messageId)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE MESSAGEID = {2}", columns, TableName,
				Prmetr.MESSAGE_ID);

			return new QueryBody(query, parameters);
		}

		public List<MessageDispatch> GetDispatchesNotReceived(string email)
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
				throw new RepoDbException("Error while executing MessageDispatchRepository.GetDispatchesNotReceived", exception);
			}
		}

		private QueryBody GetDispatchesNotReceivedMatchingEmailQuery(string email)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.EMAIL_ADDRESS, email)
			};

			string columns = GetSelectColumns(dispatchIdColumn);
			string query = string.Format("SELECT {0} FROM {1} WHERE EMAILADDRESS = {2} AND NOT MESSAGERECEIVED = 1", columns,
				TableName,
				Prmetr.EMAIL_ADDRESS);

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
				new SqlParameter(Prmetr.EMAIL_ADDRESS, receiverEmailAddress),
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
				Prmetr.EMAIL_ADDRESS,
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
			PopulateDispatch(dispatch, dbDataReader);
			return dispatch;
		}

		private void PopulateDispatch(MessageDispatch dispatch, DbDataReader dbDataReader)
		{
			dispatch.Id = dbDataReader.GetInt64(Column.ID);
			dispatch.EmailAddress = dbDataReader.GetString(Column.EMAIL_ADDRESS);
			dispatch.MessageId = dbDataReader.GetInt64(Column.MESSAGE_ID);
			dispatch.MessageReceived = dbDataReader.GetBoolean(Column.MESSAGE_RECEIVED);
			dispatch.MessageReceivedTime = dbDataReader.GetDateTime(Column.MESSAGE_RECEIVED_TIME);
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
				new SqlParameter(Prmetr.EMAIL_ADDRESS, dispatch.EmailAddress),
				new SqlParameter(Prmetr.MESSAGE_ID, dispatch.MessageId),
				new SqlParameter(Prmetr.MESSAGE_RECEIVED, GetDBValue(dispatch.MessageReceived)),
				new SqlParameter(Prmetr.MESSAGE_RECEIVED_TIME, GetDBValue(dispatch.MessageReceivedTime))
			};

			string insertStatement = string.Format(@"INSERT INTO {0}({1}, {2}, {3}, {4})", TableName,
				Column.EMAIL_ADDRESS,
				Column.MESSAGE_ID,
				Column.MESSAGE_RECEIVED,
				Column.MESSAGE_RECEIVED_TIME);

			string valueSection = string.Format(@"VALUES ({0}, {1}, {2}, {3})",
				Prmetr.EMAIL_ADDRESS,
				Prmetr.MESSAGE_ID,
				Prmetr.MESSAGE_RECEIVED,
				Prmetr.MESSAGE_RECEIVED_TIME);

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
				new SqlParameter(Prmetr.ID, dispatch.Id),
				new SqlParameter(Prmetr.EMAIL_ADDRESS, dispatch.EmailAddress),
				new SqlParameter(Prmetr.MESSAGE_ID, dispatch.MessageId),
				new SqlParameter(Prmetr.MESSAGE_RECEIVED, GetDBValue(dispatch.MessageReceived)),
				new SqlParameter(Prmetr.MESSAGE_RECEIVED_TIME, GetDBValue(dispatch.MessageReceivedTime))
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setEmailAddress = string.Format("{0} = {1}", Column.EMAIL_ADDRESS,
				Prmetr.EMAIL_ADDRESS);

			string setMessageId = string.Format("{0} = {1}", Column.MESSAGE_ID,
				Prmetr.MESSAGE_ID);

			string setMessageReceived = string.Format("{0} = {1}", Column.MESSAGE_RECEIVED,
				Prmetr.MESSAGE_RECEIVED);

			string setMessageReceivedTime = string.Format("{0} = {1}", Column.MESSAGE_RECEIVED_TIME,
				Prmetr.MESSAGE_RECEIVED_TIME);

			string whereId = string.Format("WHERE {0} = {1}", Column.ID, Prmetr.ID);

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
					new SqlParameter(Prmetr.ID, GetDBValue(dispatch.Id))
				};

				string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
					Column.ID,
					Prmetr.ID);

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
				Column.EMAIL_ADDRESS,
				Column.MESSAGE_ID,
				Column.MESSAGE_RECEIVED,
				Column.MESSAGE_RECEIVED_TIME);
			return columns;
		}

		/*protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
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
      }*/
	}
}