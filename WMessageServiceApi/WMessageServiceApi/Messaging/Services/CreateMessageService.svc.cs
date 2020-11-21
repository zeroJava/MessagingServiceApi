﻿using System;
using System.ServiceModel;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using MessageDbLib.Logging;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Authentication;

namespace WMessageServiceApi.Messaging.Services
{
	public class CreateMessageService : ICreateMessageService
    {
        public MessageRequestTokenContract CreateMessage(MessageContract message)
        {
            try
            {
				CreateMessageServiceBL createMessageL = new CreateMessageServiceBL();
				return createMessageL.CreateMessage(message);
            }
			catch (TokenValidationException exception)
			{
				WriteErrorLog("Encontered a token validation error when trying to create a message.", exception);
				ErrorContract error = new ErrorContract(exception.Message, StatusList.VALIDATION_ERROR);
				throw new FaultException<ErrorContract>(error);
			}
            catch (Exception exception)
            {
				WriteErrorLog("Encontered an error when trying to create a message.", exception);
				MessageRequestTokenContract tokenContract = CreateMessageStateTokenContract(MessageReceivedState.FailedToProcessRequest, exception.Message);
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

		private void WriteErrorLog(string message, Exception exception)
		{
			LogFile.WriteErrorLog(message, exception);
		}

		private void WriteInfoLog(string message)
		{
			LogFile.WriteInfoLog(message);
		}
	}
}