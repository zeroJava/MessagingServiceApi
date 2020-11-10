using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using MessageDbLib.Logging;

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
	}
}