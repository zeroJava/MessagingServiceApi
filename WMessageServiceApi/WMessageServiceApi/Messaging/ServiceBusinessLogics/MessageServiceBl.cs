using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.Diagnostics.Contracts;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using Transaction = MessageDbLib.DbRepositoryFactories.RepoTransactionFactory;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class MessageServiceBl : BaseBusinessLayer
	{
		public MessageRequestTokenContract CreateMessage(IMessageRequest request)
		{
			ValidateAccessToken(request.AccessToken);
			CheckMessageContent(request);
			ProcessMessage(request);
			var requestToken = new MessageRequestTokenContract
			{
				MessageRecievedState = MessageReceivedState.AcknowledgedRequest,
				Message = "Message was successfully acknowledged and persisted" +
				"in our system."
			};
			return requestToken;
		}

		private void CheckMessageContent(IMessageRequest request)
		{
			if (request.EmailAccounts == null || request.EmailAccounts.Count <= 0)
			{
				throw new InvalidOperationException("Message request does not have" +
					" any emails attached.");
			}
		}

		private void ProcessMessage(IMessageRequest request)
		{
			LogInfo("Saving request");
			User user = RetrieveUser(request.UserName);
			Message msg = new Message
			{
				MessageText = request.Message,
				SenderId = user.Id,
				SenderEmailAddress = user.EmailAddress,
				MessageCreated = request.MessageCreated
			};
			ProcessTransaction(msg, request);
			//PersistMessageToMongoDbService(msg);
		}

		private void ProcessTransaction(Message msg, IMessageRequest request)
		{
			using (IRepoTransaction repoTransaction = Transaction.GetRepoTransaction(
				DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString))
			{
				try
				{
					repoTransaction.BeginTransaction();
					PersistMessage(msg, repoTransaction);
					ProcessMessageDispatch(request, msg, repoTransaction);
					repoTransaction.Commit();
				}
				catch (Exception exception)
				{
					LogError("Could not save request to DB", exception);
					repoTransaction.Callback();
					throw;
				}
			}
		}

		private User RetrieveUser(string userName)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(
				DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(userName);
			return user ?? throw new InvalidOperationException("Sender could not" +
				"be found in our current repo");
		}

		private void PersistMessage(Message message,
			IRepoTransaction repoTransaction)
		{
			DatabaseEngineConstant databaseEngine = DatabaseOption.DatabaseEngine;
			var messageRepo = MessageRepoFactory.GetMessageRepository(databaseEngine,
				DatabaseOption.DbConnectionString,
				repoTransaction);
			messageRepo.InsertMessage(message);
			LogInfo("Message persisting was successful");
		}

		private void ProcessMessageDispatch(IMessageRequest messageContract,
			Message message,
			IRepoTransaction repoTransaction)
		{
			foreach (string emailAddress in messageContract.EmailAccounts)
			{
				MessageDispatch messageDispatch = new MessageDispatch
				{
					EmailAddress = emailAddress,
					MessageId = message.Id,
					MessageReceived = false
				};
				PersistMessagedispatch(messageDispatch, repoTransaction);
			}
		}

		private void PersistMessagedispatch(MessageDispatch messageDispatch,
			IRepoTransaction repoTransaction)
		{
			var dispatchRepo = MessageDispatchRepoFactory.GetDispatchRepository(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString,
				repoTransaction);
			dispatchRepo.InsertDispatch(messageDispatch);
			LogInfo("Message-Dispatch persisting was successful");
		}

		/*private void PersistMessageToMongoDbService(Message request)
		 {
			 try
			 {
				 RabbitMqProducerClass rabbitMqProducer = new RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
					 QueueTypeConstant.MongoDbPersistentUserService);
				 rabbitMqProducer.ExecuteMessageQueueing(request);
				 WriteInfoLog("Queueing request to Message-Queue was successful.");
			 }
			 catch (Exception exception)
			 {
				 MessageQueueErrorContract error = new MessageQueueErrorContract()
				 {
					 Message = "Error encountered when trying to queue to request queue.",
					 ExceptionMessage = exception.Message
				 };
				 WriteErrorLog("Error encountered when queueing request to Message-Queue.", exception);
				 //throw new FaultException<MessageQueueErrorContract>(error);
			 }
		 }*/
	}
}