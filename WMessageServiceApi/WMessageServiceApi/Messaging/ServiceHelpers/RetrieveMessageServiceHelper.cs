using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.Facades;

namespace WMessageServiceApi.Messaging.ServiceHelpers
{
	public class RetrieveMessageServiceHelper : BaseHelper
	{
		public List<MessageDispatchInfo> GetMessagesSentToUser(string token, IRetrieveMessageRequest messageRequest)
		{
			try
			{
				ValidToken(token);
				return RetrieveMessageFacade.GetMessagesSentToUser(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				LogError("Encontered a token validation error getting messages set to user.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ValidationError);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				LogError("Error encountered when getting messages-sent-to-user.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ProcessError);
				throw new FaultException<ErrorContract>(error);
			}
		}

		public List<MessageDispatchInfo> GetMessagesBetweenSenderReceiver(string token, IRetrieveMessageRequest messageRequest)
		{
			try
			{
				ValidToken(token);
				return RetrieveMessageFacade.GetMsgDispatchesBetweenSenderReceiver(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				LogError("Encontered a token validation error getting messages between sender and receiver.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ValidationError);
				throw new FaultException<ErrorContract>(error);
			}
			catch (Exception exception)
			{
				LogError("Error encountered when Getting-Message-Dispatches-Between-Sender-Receiver.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ProcessError);
				throw new FaultException<ErrorContract>(error);
			}
		}
	}
}