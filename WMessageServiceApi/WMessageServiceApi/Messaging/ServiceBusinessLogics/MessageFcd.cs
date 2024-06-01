using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using Transaction = MessageDbLib.DbRepositoryFactories.RepoTransactionFactory;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	/// <summary>
	/// Message facade class for saving messages to the server.
	/// </summary>
	public class MessageFcd : BaseFacade
	{
		public MessageRequestToken CreateMessage(IMessageRequest request)
		{
			ValidToken(request.AccessToken);
			ValidMessage(request);
			ProcessMessage(request);
			return new MessageRequestToken
			{
				MessageRecievedState = MessageReceivedState.Successful,
				Message = "Message was successfully acknowledged and persisted in " +
				"our system"
			};
		}

		private void ValidMessage(IMessageRequest request)
		{
			if (request.EmailAccounts != null && request.EmailAccounts.Count != 0)
			{
				return;
			}
			throw new InvalidOperationException(
				"Message request does not have any emails attached.");
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
			LogInfo("Request successful");
		}

		private void ProcessTransaction(Message msg, IMessageRequest request)
		{
			using (var transaction =
				Transaction.GetRepoTransaction(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString))
			{
				try
				{
					transaction.BeginTransaction();
					PersistMessage(msg, transaction);
					ProcessMessageDispatch(request, msg, transaction);
					transaction.Commit();
				}
				catch (Exception exception)
				{
					LogError("Could not save request to DB", exception);
					transaction.Callback();
					throw;
				}
			}
		}

		private User RetrieveUser(string userName)
		{
			var userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(userName);
			return user ?? throw new InvalidOperationException(
				"Sender could not be found in our current repo");
		}

		private void PersistMessage(Message message, IRepoTransaction repoTransaction)
		{
			var databaseEngine = DatabaseOption.DatabaseEngine;
			var messageRepo = MessageRepoFactory.GetMessageRepository(databaseEngine,
				DatabaseOption.DbConnectionString, repoTransaction);
			messageRepo.InsertMessage(message);
		}

		private void ProcessMessageDispatch(IMessageRequest messageContract,
			Message message, IRepoTransaction repoTransaction)
		{
			foreach (string emailAddress in messageContract.EmailAccounts)
			{
				var messageDispatch = new MessageDispatch
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