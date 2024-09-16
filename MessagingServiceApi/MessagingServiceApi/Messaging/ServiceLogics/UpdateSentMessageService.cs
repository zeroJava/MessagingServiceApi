using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.Message;
using System;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class UpdateSentMessageService : LogicBase
	{
		public void UpdateDispatchAsReceived(string token, long dispatchId,
			DateTime receivedDateTime)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(UpdateDispatchAsReceived));
				MessageDispatchUpdater dispatchUpdater = new MessageDispatchUpdater();
				dispatchUpdater.UpdateDispatchAsReceived(dispatchId, receivedDateTime);
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(UpdateDispatchAsReceived));
				throw processError.Invoke(exception.Message); ;
			}
		}
	}
}