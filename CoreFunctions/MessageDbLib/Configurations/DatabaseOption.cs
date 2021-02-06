using MessageDbLib.Constants;
using System;
using System.Configuration;

namespace MessageDbLib.Configurations
{
	public static class DatabaseOption
	{
		public static readonly DatabaseEngineConstant DatabaseEngine;
		//public static readonly DbContextConstant DbContextType;
		public static readonly string DbConnectionString;

		static DatabaseOption()
		{
			string appDatabaseEngineOption = ConfigurationManager.AppSettings["DatabaseEngine"];
			//string appContextType = ConfigurationManager.AppSettings["DbcontextType"];
			string appConnectionString = ConfigurationManager.AppSettings["DbConnectionString"];
			//DbContextType = appsettingValue != null && appsettingValue != string.Empty ? appsettingValue.ToUpper() : DbContextConstant.MsSqlDbContext;

			if (appDatabaseEngineOption == null || appDatabaseEngineOption == string.Empty)
			{
				throw new ApplicationException("Database option has no value assigned in the app-setting");
			}
			/*if (appContextType == null || appContextType == string.Empty)
			{
				throw new ApplicationException("Context type has no value assigned in the app-setting.");
			}*/
			if (appConnectionString == null || appConnectionString == string.Empty)
			{
				throw new ApplicationException("Connection string has no value assigned in the app-setting.");
			}

			if (!Enum.TryParse(appDatabaseEngineOption, false, out DatabaseEngineConstant databaseEngineResult))
			{
				throw new ApplicationException("Application could parse Database-Engine-Option-Constant in option-config");
			}
			/*if (!Enum.TryParse(appContextType, false, out DbContextConstant dbContextResult))
			{
				throw new ApplicationException("Application could parse Db-Context-Constant in option-config");
			}*/

			DatabaseEngine = databaseEngineResult;
			//DbContextType = dbContextResult;
			DbConnectionString = appConnectionString;

			Console.WriteLine(string.Format("DatabaseEngine: {0}", databaseEngineResult));
			//Console.WriteLine(string.Format("DbContextType: {0}", dbContextResult));
		}
	}
}