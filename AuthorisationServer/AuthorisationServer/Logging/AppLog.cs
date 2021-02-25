using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorisationServer.Logging
{
	public static class AppLog
	{
		public static void LogError(string message)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Error(message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		public static void LogInfo(string message)
		{
			try
			{
				Logger logger = LogManager.GetCurrentClassLogger();
				logger.Info(message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}