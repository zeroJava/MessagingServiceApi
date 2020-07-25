using System;
using System.Collections.Generic;
using MessageDbCore.DatabaseEngines;

namespace MessageDbLib.DbEngine
{
	public abstract class DbSqlEngine : IDbEngine
	{
		public virtual string SqlQuery { get; set; }
		protected string ConnectionString { get; set; }

		public abstract void Dispose();
		public abstract void ExecuteQuery();
		public abstract void ExecuteQueryInsertCallback<TEntity>(TEntity entity, Action<TEntity, object> populateIdCallBack);
		public abstract void ExecuteReaderQuery<TEntity>(List<TEntity> entities, PopulateResultListCallBack<TEntity> populateResultListCallBack);
	}
}
