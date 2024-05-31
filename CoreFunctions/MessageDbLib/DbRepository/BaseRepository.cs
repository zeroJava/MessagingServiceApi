using MessageDbCore.Repositories;
using MessageDbLib.DbEngine;
using MessageDbLib.Utility;
using System;
using System.Data.SqlClient;

namespace MessageDbLib.DbRepository
{
	public abstract class BaseRepository
	{
		protected readonly string connectionString;
		protected readonly IRepoTransaction repoTransaction;
		protected readonly bool transactionModeEnabled = false;

		public BaseRepository(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public BaseRepository(string connectionString,
			IRepoTransaction repoTransaction)
		{
			this.connectionString = connectionString;
			this.repoTransaction = repoTransaction;
			this.transactionModeEnabled = true;
		}

		protected virtual MssqlDbEngine GetMssqlDbEngine(string query,
			SqlParameter[] mssqlParameters,
			string connectionString)
		{
			if (transactionModeEnabled && repoTransaction != null)
			{
				MssqlDbEngine transactionMssqlEngine = new MssqlDbEngine(query,
					mssqlParameters,
					connectionString,
					repoTransaction);
				return transactionMssqlEngine;
			}

			MssqlDbEngine mssqlDbEngine = new MssqlDbEngine(query,
				mssqlParameters, connectionString);
			return mssqlDbEngine;
		}

		protected virtual MssqlDbEngine GetMssqlDbEngine(QueryBody queryBody,
			string connectionString)
		{
			string query = queryBody.Query;
			SqlParameter[] mssqlParameters = queryBody.Parameters;
			if (transactionModeEnabled && repoTransaction != null)
			{
				MssqlDbEngine transactionMssqlEngine = new MssqlDbEngine(query,
					mssqlParameters,
					connectionString,
					repoTransaction);
				return transactionMssqlEngine;
			}
			MssqlDbEngine mssqlDbEngine = new MssqlDbEngine(query,
				mssqlParameters,
				connectionString);
			return mssqlDbEngine;
		}

		protected virtual object GetDBValue(object value)
		{
			return DbValueUtil.GetValidValue(value);
		}

		protected abstract string TableName { get; set; }
	}
}