namespace MessagingServiceApi.Messaging.Services
{
	public abstract class ServiceBase
	{
		protected virtual string GetToken()
		{
			return string.Empty;
		}
	}
}