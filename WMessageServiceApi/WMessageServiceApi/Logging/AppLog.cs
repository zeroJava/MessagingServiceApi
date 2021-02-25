using NLog;
using System;

namespace WMessageServiceApi.Logging
{
	public static class AppLog
	{
		/*public static void WriteErrorLog(string errorMessage, Exception exception)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Error(string.Format(errorMessage));
				logger.Error(string.Format("\nError Message\n{0}", exception.Message));
				logger.Error(string.Format("\nStackTrace\n{0}", exception.StackTrace));
			}
			catch (Exception innerException)
			{
				//
			}
		}*/

		public static void LogError(string errorMessage)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Error(errorMessage);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
			}
		}

		public static void LogInfo(string infoMessage)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Info(infoMessage);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
			}
		}
	}
}