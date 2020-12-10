using System.Data.SqlClient;

namespace MessageDbLib.DbRepository
{
    public sealed class QueryBody
    {
        public string Query { get; private set; }
        public SqlParameter[] Parameters { get; private set; }

        public QueryBody(string query, SqlParameter[] parameters)
        {
            Query = query;
            Parameters = parameters;
        }
    }
}