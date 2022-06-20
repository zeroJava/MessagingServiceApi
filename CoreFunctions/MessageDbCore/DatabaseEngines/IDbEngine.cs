using System;
using System.Collections.Generic;
using System.Data.Common;

namespace MessageDbCore.DatabaseEngines
{
   public delegate TEntity PopulateResultListCallBack<TEntity>(DbDataReader dbDataReader);
   public interface IDbEngine : IDisposable
   {
      string SqlQuery { get; set; }
      void ExecuteQuery();
      void ExecuteQueryInsertCallback<TEntity>(TEntity entity, Action<TEntity, object> populateIdCallBack);
      void ExecuteReaderQuery<TEntity>(List<TEntity> entities, PopulateResultListCallBack<TEntity> populateResultListCallBack);
   }
}
