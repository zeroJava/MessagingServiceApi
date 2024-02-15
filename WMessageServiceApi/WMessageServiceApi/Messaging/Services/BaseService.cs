namespace WMessageServiceApi.Messaging.Services
{
	public abstract class BaseService
	{
		protected virtual void LogError(string message)
		{
			Logging.AppLog.LogError(message);
		}
	}
}