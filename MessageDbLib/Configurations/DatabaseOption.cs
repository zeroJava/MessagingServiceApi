using MessageDbLib.Constants;
using MessageDbLib.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageDbLib.Configurations
{
    public static class DatabaseOption
    {
        public static readonly DatabaseEngineConstant DatabaseEngine;
        //public static readonly DbContextConstant DbContextType;
        public static readonly string DbConnectionString;

        static DatabaseOption()
        {
            try
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
                WriteInfoLog("DatabaseOptionConfig was successful.");
            }
            catch (ApplicationException exception)
            {
                string message = exception.Message;
                string innerMessage = exception.InnerException != null ? exception.InnerException.Message : "No-Inner-Exception-Message";
                WriteErrorLog($"Error encountered, message: {message};\ninner message: {innerMessage}");
                throw;
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                string innerMessage = exception.InnerException != null ? exception.InnerException.Message : "No-Inner-Exception-Message";
                WriteErrorLog(string.Format("Encountered error when load database options.\n{0}\nInner message\n{1}", message,
                    innerMessage));
                throw;
            }
        }

        private static void WriteErrorLog(string errorMessage)
        {
            LogFile.WriteErrorLog(errorMessage);
        }

        private static void WriteInfoLog(string infoMessage)
        {
            LogFile.WriteInfoLog(infoMessage);
        }
    }
}