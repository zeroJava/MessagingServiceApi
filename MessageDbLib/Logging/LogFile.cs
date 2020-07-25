using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageDbLib.Logging
{
	public static class LogFile
	{
        public static void WriteErrorLog(string errorMessage, Exception exception)
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
		}

		public static void WriteErrorLog(string errorMessage)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Error(errorMessage);
			}
			catch (Exception exception)
			{
				//
			}
		}

		public static void WriteInfoLog(string infoMessage)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Info(infoMessage);
			}
			catch (Exception exception)
			{
				//
			}
		}
	}
}