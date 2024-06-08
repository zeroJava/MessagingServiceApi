using NLog;
using System;

namespace MessagingServiceFunctions
{
	public static class Log
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

		public static void Error(string errorMessage)
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

		public static void Info(string infoMessage)
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