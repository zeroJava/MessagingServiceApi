using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.DbEngine;
using MessageDbLib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Column = MessageDbLib.Constants.TableConstants.OrganisationKeyColumn;
using Prmetr = MessageDbLib.Constants.TableConstants.OrganisationKeyParameter;

namespace MessageDbLib.DbRepository.ADO.MsSql
{
	public class OrganisationKeyRepository : BaseRepository, IOrganisationKeyRepository
	{
		//protected readonly string connectionString;
		//protected readonly IRepoTransaction repoTransaction;
		//protected readonly bool transactionModeEnabled = false;
		protected override string TableName { get; set; } = "dbo.OrganisationKeyTable";

		public OrganisationKeyRepository(string connectionString) :
			base(connectionString)
		{
			//this.connectionString = connectionString;
		}

		public OrganisationKeyRepository(string connectionString,
			IRepoTransaction repoTransaction) : base(connectionString,
				repoTransaction)
		{
			//this.connectionString = connectionString;
			//this.repoTransaction = repoTransaction;
			//this.transactionModeEnabled = true;
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
				throw new RepoDbException("Error while executing OrganisationKeyRepository.GetOrganisationKeyMatchingName", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetOrganisationKeyMatchingNameQuery(string name)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.NAME, name)
			};

			string columns = GetSelectColumns();
			string query = string.Format("SELECT {0} FROM {1} WHERE NAME = {2}", columns, TableName,
				Prmetr.NAME);

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
			organisationKey.Id = dataReader.GetInt64(Column.ID);
			organisationKey.Name = dataReader.GetString(Column.NAME);
			organisationKey.OKey = dataReader.GetString(Column.OKEY);
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
				throw new RepoDbException("Error while executing OrganisationKeyRepository.InsertOrganisationKey", exception);
			}
		}

		private Tuple<string, SqlParameter[]> InsertOrganisationQuery(OrganisationKey organisationKey)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.NAME, GetDBValue(organisationKey.Name)),
				new SqlParameter(Prmetr.OKEY, GetDBValue(organisationKey.OKey)),
			};

			string insert = string.Format("INSERT INTO {0}({1}, {2})", TableName,
				Column.NAME,
				Column.OKEY);

			string values = string.Format("VALUES ({0}, {1})", Prmetr.NAME,
				Prmetr.OKEY);

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
				throw new RepoDbException("Error while executing OrganisationKeyRepository.UpdateOrganisationKey", exception);
			}
		}

		private Tuple<string, SqlParameter[]> GetUpdateOrganisationKeyQuery(OrganisationKey organisationKey)
		{
			SqlParameter[] parameters = new SqlParameter[]
			{
				new SqlParameter(Prmetr.ID, GetDBValue(organisationKey.Id)),
				new SqlParameter(Prmetr.NAME, GetDBValue(organisationKey.Name)),
				new SqlParameter(Prmetr.OKEY, GetDBValue(organisationKey.OKey)),
			};

			string updateTable = string.Format("UPDATE {0} SET", TableName);

			string setName = string.Format("{0} = {1}", Column.NAME,
				Prmetr.NAME);

			string setOKey = string.Format("{0} = {1}", Column.OKEY,
				Prmetr.OKEY);

			string whereId = string.Format("WHERE {0} = {1}", Column.ID,
				Prmetr.ID);

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
					new SqlParameter(Prmetr.ID, GetDBValue(organisationKey.Id))
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
				throw new RepoDbException("Error while executing OrganisationKeyRepository.DeleteOrganisationKey", exception);
			}
		}

		protected static string GetSelectColumns()
		{
			string columns = string.Format("{0}, {1}, {2}", Column.ID,
				Column.NAME,
				Column.OKEY);
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