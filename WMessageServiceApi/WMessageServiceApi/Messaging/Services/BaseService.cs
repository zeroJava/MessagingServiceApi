namespace WMessageServiceApi.Messaging.Services
{
	public abstract class BaseService
	{
		protected virtual string GetToken()
		{
			return string.Empty;
		}
	}
}