using System;
using System.ServiceModel;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class CreateMessageService : BaseService, ICreateMessageService
	{
		public MessageRequestToken CreateMessage(MessageRequest message)
		{
			try
			{
				MessageLogic createMessageFacade = new MessageLogic();
				return createMessageFacade.CreateMessage(message);
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
					MessageRecievedState = MessageReceivedState.FailedToProcessRequest,
					Message = exception.Message,
				};
				throw new FaultException<MessageRequestToken>(tokenContract);
			}
		}
	}
}