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

namespace MessageDbLib.DbRepository.ADO
{
	public class OrganisationKeyRepositoryMsSql : IOrganisationKeyRepository
	{
		protected readonly string connectionString;
		protected readonly IRepoTransaction repoTransaction;
		protected readonly bool transactionModeEnabled = false;
		public virtual string TableName { get; protected set; } = "dbo.OrganisationKeyTable";

		public OrganisationKeyRepositoryMsSql(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public OrganisationKeyRepositoryMsSql(string connectionString, IRepoTransaction repoTransaction)
		{
			this.connectionString = connectionString;
			this.repoTransaction = repoTransaction;
			this.transactionModeEnabled = true;
		}

		public OrganisationKey GetOrganisationKeyMatchingName(string name)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = GetOrganisationKeyMatchingNameQuery(name);
				using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					List<OrganisationKey> organisationKies = new List<OrganisationKey>();
					dbEngine.ExecuteReaderQuery(organisationKies, OnPopulateResultListCallBack);
					return organisationKies.FirstOrDefault();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing OrganisationKeyRepositoryMsSql.GetOrganisationKeyMatchingName", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetOrganisationKeyMatchingNameQuery(string name)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(OrganisationKeyParameter.NAME, name)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE NAME = {2}", columns, TableName,
				OrganisationKeyParameter.NAME);
			return Tuple.Create(query, parameters);
		}

		private OrganisationKey OnPopulateResultListCallBack(DbDataReader dataReader)
		{
			OrganisationKey organisationKey = new OrganisationKey();
			PopulateOrganisationKey(organisationKey, dataReader);
			return organisationKey;
		}

		private void PopulateOrganisationKey(OrganisationKey organisationKey, DbDataReader dataReader)
		{
			string idColumn = dataReader[OrganisationKeyColumn.ID] != null ? dataReader[OrganisationKeyColumn.ID].ToString() :
				string.Empty;

			long id;
			if (!string.IsNullOrEmpty(idColumn) &&
				long.TryParse(idColumn, out id))
			{
				organisationKey.Id = id;
			}

			if (dataReader[OrganisationKeyColumn.NAME] != null)
			{
				organisationKey.Name = dataReader[OrganisationKeyColumn.NAME].ToString();
			}

			if (dataReader[OrganisationKeyColumn.OKEY] != null)
			{
				organisationKey.OKey = dataReader[OrganisationKeyColumn.OKEY].ToString();
			}
		}

		public void InsertOrganisationKey(OrganisationKey organisationKey)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = InsertOrganisationQuery(organisationKey);
				using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					dbEngine.ExecuteQueryInsertCallback(organisationKey, OnPopulateIdCallBack);
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing OrganisationKeyRepositoryMsSql.InsertOrganisationKey", exception);
			}
		}

		private Tuple<string, SqlParameter[]> InsertOrganisationQuery(OrganisationKey organisationKey)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(OrganisationKeyParameter.NAME, GetDBValue(organisationKey.Name)),
				new SqlParameter(OrganisationKeyParameter.OKEY, GetDBValue(organisationKey.OKey)),
			};

			string insert = string.Format("INSERT INTO {0}({1}, {2})", TableName,
				OrganisationKeyColumn.NAME,
				OrganisationKeyColumn.OKEY);

			string values = string.Format("VALUES ({0}, {1})", OrganisationKeyParameter.NAME,
				OrganisationKeyParameter.OKEY);

			string stringquery = string.Format("{0} {1}", insert, values);
			return Tuple.Create(stringquery, parameters);
		}

		private void OnPopulateIdCallBack(OrganisationKey organisationKey, object result)
		{
			long id = Convert.ToInt64(result);
			organisationKey.Id = id;
		}

		public void UpdateOrganisationKey(OrganisationKey organisationKey)
		{
			try
			{
				Tuple<string, SqlParameter[]> query = GetUpdateOrganisationKeyQuery(organisationKey);
				using (MssqlDbEngine dbEngine = GetMssqlDbEngine(query.Item1, query.Item2,
					connectionString))
				{
					dbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing OrganisationKeyRepositoryMsSql.UpdateOrganisationKey", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUpdateOrganisationKeyQuery(OrganisationKey organisationKey)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(OrganisationKeyParameter.ID, GetDBValue(organisationKey.Id)),
				new SqlParameter(OrganisationKeyParameter.NAME, GetDBValue(organisationKey.Name)),
				new SqlParameter(OrganisationKeyParameter.OKEY, GetDBValue(organisationKey.OKey)),
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setName = string.Format("{0} = {1}", OrganisationKeyColumn.NAME,
				OrganisationKeyParameter.NAME);

			string setOKey = string.Format("{0} = {1}", OrganisationKeyColumn.OKEY,
				OrganisationKeyParameter.OKEY);

			string whereId = string.Format("WHERE {0} = {1}", OrganisationKeyColumn.ID,
				OrganisationKeyParameter.ID);

			string query = string.Format("{0} {1}, {2} {3}", updateTable, setName, setOKey,
				whereId);
			return Tuple.Create(query, parameters);
		}

		public void DeleteOrganisationKey(OrganisationKey organisationKey)
		{
			try
			{
				SqlParameter[] sqlParameters = new SqlParameter[]
				{
					new SqlParameter(OrganisationKeyParameter.ID, GetDBValue(organisationKey.Id))
				};

				string sqlQuery = string.Format("DELETE FROM {0} WHERE {1} = {2}", TableName,
					OrganisationKeyColumn.ID,
					OrganisationKeyParameter.ID);
				using (MssqlDbEngine mssqlDbEngine = GetMssqlDbEngine(sqlQuery, sqlParameters,
					connectionString))
				{
					mssqlDbEngine.ExecuteQuery();
				}
			}
			catch (Exception exception)
			{
				throw new RepoDbException("Error while executing OrganisationKeyRepositoryMsSql.DeleteOrganisationKey", exception);
			}
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}", OrganisationKeyColumn.ID,
				OrganisationKeyColumn.NAME,
				OrganisationKeyColumn.OKEY);
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

		protected object GetDBValue(object value)
		{
			return DbValueUtil.GetValidValue(value);
		}
	}
}