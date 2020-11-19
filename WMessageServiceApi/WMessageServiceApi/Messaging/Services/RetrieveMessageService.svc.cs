using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using MessageDbLib.Logging;
using WMessageServiceApi.Authentication;

namespace WMessageServiceApi.Messaging.Services
{
	public class RetrieveMessageService : IRetrieveMessageService
	{
		public List<MessageDispatchInfoContract> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			try
			{
				RetrieveMessageServiceBL retrieveMessageBL = new RetrieveMessageServiceBL();
				return retrieveMessageBL.GetMessagesSentToUser(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				WriteErrorLog("Encontered a token validation error getting messages set to user.", exception);
				ValidationErrorContract tokenContract = CreateValidationErrorContract(exception);
				throw new FaultException<ValidationErrorContract>(tokenContract);
			}
			catch (Exception exception)
			{
				EntityErrorContract error = new EntityErrorContract
				{
					Message = exception.Message
				};
				WriteErrorLog("Error encountered when getting messages-sent-to-user.", exception);
				throw new FaultException<EntityErrorContract>(error);
			}
		}

		public List<MessageDispatchInfoContract> GetMessageDipatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest)
		{
			try
			{
				RetrieveMessageServiceBL retrieveMessageBL = new RetrieveMessageServiceBL();
				return retrieveMessageBL.GetMsgDispatchesBetweenSenderReceiver(messageRequest);
			}
			catch (TokenValidationException exception)
			{
				WriteErrorLog("Encontered a token validation error getting messages between sender and receiver.", exception);
				ValidationErrorContract tokenContract = CreateValidationErrorContract(exception);
				throw new FaultException<ValidationErrorContract>(tokenContract);
			}
			catch (Exception exception)
			{
				EntityErrorContract error = new EntityErrorContract
				{
					Message = exception.Message
				};
				WriteErrorLog("Error encountered when Getting-Message-Dispatches-Between-Sender-Receiver.", exception);
				throw new FaultException<EntityErrorContract>(error);
			}
		}

		private void WriteErrorLog(string message, Exception exception)
		{
			LogFile.WriteErrorLog(message, exception);
		}

		private void WriteInfoLog(string message)
		{
			LogFile.WriteInfoLog(message);
		}

		private ValidationErrorContract CreateValidationErrorContract(TokenValidationException exception)
		{
			return new ValidationErrorContract
			{
				Message = exception.Message,
				Reason = exception.Reason,
			};
		}
	}
}