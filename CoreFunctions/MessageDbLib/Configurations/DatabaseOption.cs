using MessageDbLib.Constants;
using System;
using System.Configuration;

namespace MessageDbLib.Configurations
{
	public static class DatabaseOption
	{
		public static readonly DatabaseEngineConstant DatabaseEngine;
		public static readonly string DbConnectionString;

		static DatabaseOption()
		{
			string appDatabaseEngineOption = ConfigurationManager.AppSettings["DatabaseEngine"];
			string appConnectionString = ConfigurationManager.AppSettings["DbConnectionString"];

			if (appDatabaseEngineOption == null || appDatabaseEngineOption == string.Empty)
			{
				throw new ApplicationException("Database option has no value assigned in the app-setting");
			}

			if (appConnectionString == null || appConnectionString == string.Empty)
			{
				throw new ApplicationException("Connection string has no value assigned in the app-setting.");
			}

			if (!Enum.TryParse(appDatabaseEngineOption, false, out DatabaseEngineConstant databaseEngineResult))
			{
				throw new ApplicationException("Application could parse Database-Engine-Option-Constant in option-config");
			}

			DatabaseEngine = databaseEngineResult;
			DbConnectionString = appConnectionString;

			Console.WriteLine(string.Format("DatabaseEngine: {0}", databaseEngineResult));
		}
	}
}