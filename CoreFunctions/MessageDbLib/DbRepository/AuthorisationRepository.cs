using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Column = MessageDbLib.Constants.TableConstants.AuthorisationColumn;
using Prmetr = MessageDbLib.Constants.TableConstants.AuthorisationParameter;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class AuthorisationRepository : BaseRepository, IAuthorisationRepository
	{
		//protected readonly string connectionString;
		//protected readonly IRepoTransaction repoTransaction;
		//protected readonly bool transactionModeEnabled = false;

		public virtual string TableName { get; protected set; } = "dbo.AuthorisationTable";

		public AuthorisationRepository(string connectionString) :
			base(connectionString)
		{
			//this.connectionString = connectionString;
		}

		public AuthorisationRepository(string connectionString,
			IRepoTransaction repoTransaction) : base(connectionString,
				repoTransaction)
		{
			//this.connectionString = connectionString;
			//this.repoTransaction = repoTransaction;
			//this.transactionModeEnabled = true;
		}

		public Authorisation GetAuthorisationMatchingAuthCode(Guid guid)
		{
			try
			{
				QueryBody query = GetAuthorisationMatchingAuthCodeQuery(guid);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					List<Authorisation> authorisations = new List<Authorisation>();
					mssqlDbEngine.ExecuteReaderQuery(authorisations,
						OnPopulateResultListCallBack);
					return authorisations.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.GetAuthorisationMatchingAuthCode", exception);
			}
		}

		private QueryBody GetAuthorisationMatchingAuthCodeQuery(Guid guid)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.AUTHORISATION_CODE, guid),
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE " +
				"AUTHORISATIONCODE = {2}",
				columns,
				TableName,
				Prmetr.AUTHORISATION_CODE);

			return new QueryBody(query, parameters);
		}

		public Authorisation GetAuthorisationMatchingId(long id)
		{
			try
			{
				QueryBody query = GetAuthorisationMatchingIdQuery(id);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					List<Authorisation> authorisations = new List<Authorisation>();
					mssqlDbEngine.ExecuteReaderQuery(authorisations,
						OnPopulateResultListCallBack);
					return authorisations.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.GetAuthorisationMatchingId", exception);
			}
		}

		private QueryBody GetAuthorisationMatchingIdQuery(long id)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, id),
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE ID = {2}",
				columns,
				TableName,
				Prmetr.ID);

			return new QueryBody(query, parameters);
		}

		public List<Authorisation> GetAuthorisationsGreaterThanEndTime(DateTime endtime)
		{
			try
			{
				QueryBody query = GetAuthorisationGreaterThanEndTimeQuery(endtime);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					List<Authorisation> authorisations = new List<Authorisation>();
					mssqlDbEngine.ExecuteReaderQuery(authorisations, OnPopulateResultListCallBack);
					return authorisations;
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.GetAuthorisationsGreaterThanEndTime", exception);
			}
		}

		private QueryBody GetAuthorisationGreaterThanEndTimeQuery(DateTime dateTime)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.END_TIME, dateTime),
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE ENDTIME >= {2}",
				columns,
				TableName,
				Prmetr.END_TIME);

			return new QueryBody(query, parameters);
		}

		public void InsertAuthorisation(Authorisation authorisation)
		{
			try
			{
				QueryBody query = GetInsertAuthorisationQuery(authorisation);
				using (DbSqlEngine sqlEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					sqlEngine.ExecuteQueryInsertCallback(authorisation,
						OnPopulatedIdCallback);
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.InsertAuthorisation", exception);
			}
		}

		private QueryBody GetInsertAuthorisationQuery(Authorisation authorisation)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.AUTHORISATION_CODE,
					GetDBValue(authorisation.AuthorisationCode)),
				new SqlParameter(Prmetr.START_TIME,
					GetDBValue(authorisation.StartTime)),
				new SqlParameter(Prmetr.END_TIME,
					GetDBValue(authorisation.EndTime)),
				new SqlParameter(Prmetr.USER_ID,
					GetDBValue(authorisation.UserId)),
			};

			string insertSection = string.Format("INSERT INTO {0}({1}, {2}, {3}, " +
				"{4})",
				TableName,
				Column.AUTHORISATION_CODE,
				Column.START_TIME,
				Column.END_TIME,
				Column.USER_ID);

			string valueSection = string.Format("VALUES ({0}, {1}, {2}, {3})",
				Prmetr.AUTHORISATION_CODE,
				Prmetr.START_TIME,
				Prmetr.END_TIME,
				Prmetr.USER_ID);

			string query = string.Format("{0} {1}", insertSection, valueSection);

			return new QueryBody(query, parameters);
		}

		private void OnPopulatedIdCallback(Authorisation authorisation,
			object result)
		{
			long id = Convert.ToInt64(result);
			authorisation.Id = id;
		}

		public void UpdateAuthorisation(Authorisation authorisation)
		{
			try
			{
				QueryBody query = GetUpdateAuthorisationQuery(authorisation);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.UpdateAuthorisation", exception);
			}
		}

		private QueryBody GetUpdateAuthorisationQuery(Authorisation authorisation)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID,
					GetDBValue(authorisation.Id)),
				new SqlParameter(Prmetr.AUTHORISATION_CODE,
					GetDBValue(authorisation.AuthorisationCode)),
				new SqlParameter(Prmetr.START_TIME,
					GetDBValue(authorisation.StartTime)),
				new SqlParameter(Prmetr.END_TIME,
					GetDBValue(authorisation.EndTime)),
				new SqlParameter(Prmetr.USER_ID,
					GetDBValue(authorisation.UserId)),
			};

			string updateStr = string.Format("UPDATE {0} SET", TableName);

			string setAuthCode = string.Format("{0} = {1}",
				Column.AUTHORISATION_CODE,
				Prmetr.AUTHORISATION_CODE);
			string setStartTime = string.Format("{0} = {1}",
				Column.START_TIME,
				Prmetr.START_TIME);
			string setEndTime = string.Format("{0} = {1}",
				Column.END_TIME,
				Prmetr.END_TIME);
			string setUserId = string.Format("{0} = {1}",
				Column.USER_ID,
				Prmetr.USER_ID);

			string whereId = string.Format("WHERE {0} = {1}",
				Column.ID,
				GetDBValue(authorisation.Id));

			string query = string.Format("{0} {1}, {2}, {3}, {4} {5}",
				updateStr,
				setAuthCode,
				setStartTime,
				setEndTime,
				setUserId,
				whereId);

			return new QueryBody(query, parameters);
		}

		public void DeleteAuthorisation(Authorisation authorisation)
		{
			try
			{
				QueryBody query = GetDeletionAuthorisationQuery(authorisation);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(query.Query,
					query.Parameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing Authorisation" +
					"Repository.DeleteAuthorisation", exception);
			}
		}

		private QueryBody GetDeletionAuthorisationQuery(Authorisation authorisation)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, GetDBValue(authorisation.Id)),
			};

			string query = string.Format("DELETE FROM {0} WHERE ID = {1}", TableName,
				Prmetr.ID);

			return new QueryBody(query, parameters);
		}

		public void Dispose()
		{
			//
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}, {3}, {4}",
				Column.ID,
				Column.AUTHORISATION_CODE,
				Column.START_TIME,
				Column.END_TIME,
				Column.USER_ID);
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
		}*/

		private Authorisation OnPopulateResultListCallBack(DbDataReader dataReader)
		{
			Authorisation authorisation = new Authorisation();
			PopulateAuthorisation(authorisation, dataReader);
			return authorisation;
		}

		private void PopulateAuthorisation(Authorisation authorisation, DbDataReader dataReader)
		{
			authorisation.Id = dataReader.GetInt64(Column.ID);
			authorisation.AuthorisationCode = dataReader.GetGuid(Column.AUTHORISATION_CODE);
			authorisation.StartTime = dataReader.GetDateTime(Column.START_TIME);
			authorisation.EndTime = dataReader.GetDateTime(Column.END_TIME);
			authorisation.UserId = dataReader.GetInt64(Column.USER_ID);
		}

		/*protected object GetDBValue(object value)
		{
			return DbValueUtil.GetValidValue(value);
		}*/
	}
}