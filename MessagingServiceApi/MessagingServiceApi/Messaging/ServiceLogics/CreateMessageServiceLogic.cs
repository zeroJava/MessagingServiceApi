using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.Message;
using MessagingServiceInterfaces.Contracts.Message;
using System;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class CreateMessageServiceLogic : LogicBase
	{
		public MessageRequestToken CreateMessage(string token,
			MessageRequest message)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(CreateMessage));
				MessageCreator messageCreator = new MessageCreator();
				return messageCreator.Create(message);
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(CreateMessage));
				throw messageError.Invoke(exception.ToString());
			}
		}
	}
}