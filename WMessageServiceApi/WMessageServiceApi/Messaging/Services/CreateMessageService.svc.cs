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
	public class CreateMessageService : ICreateMessageService
	{
		public MessageRequestTokenContract CreateMessage(MessageContract message)
		{
			try
			{
				return new CreateMessageFacade().CreateMessage(message);
			}
			catch (TokenValidationException exception)
			{
				string exMessage = "Encontered a token validation error when trying to create a message.";

				LogError(exMessage + "\n" + exception.ToString());

				ErrorContract error = new ErrorContract(exMessage, StatusList.VALIDATION_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				string exMessage = "Encontered an error when trying to create a message.";

				LogError(exMessage + "\n" + exception.ToString());

				MessageRequestTokenContract tokenContract = CreateMessageStateTokenContract(MessageReceivedState.FailedToProcessRequest,
					 exception.Message);
				throw new FaultException<MessageRequestTokenContract>(tokenContract);
			}
		}

		private MessageRequestTokenContract CreateMessageStateTokenContract(MessageReceivedState recievedState, string message)
		{
			return new MessageRequestTokenContract
			{
				MessageRecievedState = recievedState,
				Message = message
			};
		}

		private void LogError(string message)
		{
			Logging.AppLog.LogError(message);
		}
	}
}