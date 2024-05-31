using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Column = MessageDbLib.Constants.TableConstants.AccessColumn;
using Prmetr = MessageDbLib.Constants.TableConstants.AccessParameter;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class AccessRepository : BaseRepository, IAccessRepository
	{
		//protected readonly string connectionString;
		//protected readonly IRepoTransaction repoTransaction;
		//protected readonly bool transactionModeEnabled = false;

		protected override string TableName { get; set; } = "dbo.AccessTable";

		public AccessRepository(string connectionString) : base(connectionString)
		{
			//this.connectionString = connectionString;
		}

		public AccessRepository(string connectionString, IRepoTransaction repoTransaction) :
			base(connectionString, repoTransaction)
		{
			//this.connectionString = connectionString;
			//this.repoTransaction = repoTransaction;
			//this.transactionModeEnabled = true;
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
				throw new RepoDbException("Error while executing AccessRepository.GetAccessMatchingToken", exception);
			}
		}

		private QueryBody GetAccessMatchingTokenQuery(string token)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.TOKEN, token)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE TOKEN = {2}", columns,
				TableName,
				Prmetr.TOKEN);

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
				throw new RepoDbException("Error while executing AccessRepository.GetAccessMatchingId", exception);
			}
		}

		private QueryBody GetAccessMatchingIdQuery(long id)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, id)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}", columns,
				TableName,
				Prmetr.ID);

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
			access.Id = dataReader.GetInt64(Column.ID);
			access.Organisation = dataReader.GetString(Column.ORGANISATION);
			access.Token = dataReader.GetString(Column.TOKEN);
			access.UserId = dataReader.GetInt64(Column.USER_ID);
			access.StartTime = dataReader.GetDateTime(Column.START_TIME);
			access.EndTime = dataReader.GetDateTime(Column.END_TIME);

			string scopeColumn = dataReader.GetString(Column.SCOPE);
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
				throw new RepoDbException("Error while executing AccessRepository.InsertAccess", exception);
			}
		}

		private QueryBody GetInsertAccessQuery(Access access)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ORGANISATION, GetDBValue(access.Organisation)),
				new SqlParameter(Prmetr.TOKEN, GetDBValue(access.Token)),
				new SqlParameter(Prmetr.USER_ID, GetDBValue(access.UserId)),
				new SqlParameter(Prmetr.START_TIME, GetDBValue(access.StartTime)),
				new SqlParameter(Prmetr.END_TIME, GetDBValue(access.EndTime)),
				new SqlParameter(Prmetr.SCOPE, GetDBValue(JoinScopeList(access.Scope))),
			};

			string insert = string.Format("INSERT INTO {0}({1}, {2}, {3}, {4}, {5}, {6})", TableName,
				Column.ORGANISATION,
				Column.TOKEN,
				Column.USER_ID,
				Column.START_TIME,
				Column.END_TIME,
				Column.SCOPE);

			string values = string.Format("VALUES ({0}, {1}, {2}, {3}, {4}, {5})", Prmetr.ORGANISATION,
				Prmetr.TOKEN,
				Prmetr.USER_ID,
				Prmetr.START_TIME,
				Prmetr.END_TIME,
				Prmetr.SCOPE);

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
				throw new RepoDbException("Error while executing AccessRepository.UpdateAccess", exception);
			}
		}

		private QueryBody GetUpdateAccessQuery(Access access)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, GetDBValue(access.Id)),
				new SqlParameter(Prmetr.ORGANISATION, GetDBValue(access.Organisation)),
				new SqlParameter(Prmetr.TOKEN, GetDBValue(access.Token)),
				new SqlParameter(Prmetr.USER_ID, GetDBValue(access.UserId)),
				new SqlParameter(Prmetr.START_TIME, GetDBValue(access.StartTime)),
				new SqlParameter(Prmetr.END_TIME, GetDBValue(access.EndTime)),
				new SqlParameter(Prmetr.SCOPE, GetDBValue(JoinScopeList(access.Scope))),
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setOrganisation = string.Format("{0} = {1}", Column.ORGANISATION,
				Prmetr.ORGANISATION);

			string setToken = string.Format("{0} = {1}", Column.TOKEN,
				Prmetr.TOKEN);

			string setUserId = string.Format("{0} = {1}", Column.USER_ID,
				Prmetr.USER_ID);

			string setStartTime = string.Format("{0} = {1}", Column.START_TIME,
				Prmetr.START_TIME);

			string setEndTime = string.Format("{0} = {1}", Column.END_TIME,
				Prmetr.END_TIME);

			string setScope = string.Format("{0} = {1}", Column.SCOPE,
				Prmetr.SCOPE);

			string whereId = string.Format("WHERE {0} = {1}", Column.ID,
				Prmetr.ID);

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
					new SqlParameter(Prmetr.ID, GetDBValue(access.Id))
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
				throw new RepoDbException("Error while executing AccessRepository.DeleteAccess", exception);
			}
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
				Column.ID,
				Column.ORGANISATION,
				Column.TOKEN,
				Column.USER_ID,
				Column.START_TIME,
				Column.END_TIME,
				Column.SCOPE);
			return columns;
		}

		/*protected MssqlDbEngine GetMssqlDbEngine(string query, SqlParameter[] mssqlParameters,
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

		protected object GetDBValue(object value)
		{
			return DbValueUtil.GetValidValue(value);
		}*/
	}
}