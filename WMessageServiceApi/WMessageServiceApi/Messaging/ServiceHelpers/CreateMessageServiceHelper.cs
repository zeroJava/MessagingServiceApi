using System;
using System.ServiceModel;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using WMessageServiceApi.Messaging.Facades;

namespace WMessageServiceApi.Messaging.ServiceHelpers
{
	public class CreateMessageServiceHelper : BaseHelper
	{
		public MessageRequestToken CreateMessage(string token, MessageRequest message)
		{
			try
			{
				ValidToken(token);
				LogInfo($"{nameof(CreateMessage)} invoked");
				return MessageFacade.CreateMessage(message);
			}
			catch (TokenValidationException exception)
			{
				LogError("Error validationg token\n" + exception.ToString());
				ErrorContract error = new ErrorContract("Error validating token",
					StatusList.ValidationError);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				LogError("Error creating message\n" + exception.ToString());
				var tokenContract = new MessageRequestToken
				{
					MessageRecievedState = MessageReceivedState.FailedToProcess,
					Message = exception.Message,
				};
				throw new FaultException<MessageRequestToken>(tokenContract);
			}
		}
	}
}