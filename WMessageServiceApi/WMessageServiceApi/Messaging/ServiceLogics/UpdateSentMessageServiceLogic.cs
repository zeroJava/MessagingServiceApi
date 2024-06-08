using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.Message;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Errors;
using System;
using System.ServiceModel;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class UpdateSentMessageServiceLogic : LogicBase
	{
		public void UpdateDispatchAsReceived(string token, long dispatchId,
			DateTime receivedDateTime)
		{
			try
			{
				ValidateToken(token);
				LogMethodICalled(nameof(UpdateDispatchAsReceived));
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