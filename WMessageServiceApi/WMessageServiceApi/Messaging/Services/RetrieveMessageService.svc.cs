using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Logging;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class RetrieveMessageService : IRetrieveMessageService
	{
		public List<MessageDispatchInfoContract> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			try
			{
				RetrieveMessageServiceFacade retrieveMessageBL = new RetrieveMessageServiceFacade();
				return retrieveMessageBL.GetMessagesSentToUser(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				LogError("Encontered a token validation error getting messages set to user.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.VALIDATION_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				LogError("Error encountered when getting messages-sent-to-user.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.PROCESS_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
		}

		public List<MessageDispatchInfoContract> GetMessageDipatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest)
		{
			try
			{
				RetrieveMessageServiceFacade retrieveMessageBL = new RetrieveMessageServiceFacade();
				return retrieveMessageBL.GetMsgDispatchesBetweenSenderReceiver(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				LogError("Encontered a token validation error getting messages between sender and receiver.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.VALIDATION_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				LogError("Error encountered when Getting-Message-Dispatches-Between-Sender-Receiver.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.PROCESS_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
		}

		private void LogError(string message, Exception exception)
		{
			AppLog.LogError(message + "\n" + exception.ToString());
		}
	}
}