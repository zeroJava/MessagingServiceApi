using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using System;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using Transaction = MessageDbLib.DbRepositoryFactories.RepoTransactionFactory;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class CreateMessageFacade : BaseFacade
	{
		public MessageRequestTokenContract CreateMessage(IMessageContract contract)
		{
			ValidateAccessToken(contract.AccessToken);
			CheckMessageContent(contract);

			LogInfo("Saving contract");
			User user = RetrieveUser(contract.UserName);
			Message msg = new Message
			{
				MessageText = contract.Message,
				SenderId = user.Id,
				SenderEmailAddress = user.EmailAddress,
				MessageCreated = contract.MessageCreated
			};
			using (IRepoTransaction repoTransaction = Transaction.GetRepoTransaction(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString))
			{
				try
				{
					repoTransaction.BeginTransaction();
					PersistMessage(msg, repoTransaction);
					ProcessMessageDispatch(contract, msg, repoTransaction);
					repoTransaction.Commit();
				}
				catch (Exception exception)
				{
					LogError("Could not save message to DB", exception);
					repoTransaction.Callback();
					throw;
				}
			}
			//PersistMessageToMongoDbService(msg);
			var requestToken = new MessageRequestTokenContract
			{
				MessageRecievedState = MessageReceivedState.AcknowledgedRequest,
				Message = "Message was successfully acknowledged and persisted" +
				"in our system."
			};
			return requestToken;
		}

		private void CheckMessageContent(IMessageContract message)
		{
			if (message.EmailAccounts == null || message.EmailAccounts.Count <= 0)
			{
				throw new InvalidOperationException("Message contract does not have" +
					" any emails attahed.");
			}
		}

		private User RetrieveUser(string userName)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(userName);
			return user ?? throw new InvalidOperationException("Sender could not" +
				"be found in our current repo");
		}

		private void PersistMessage(Message message,
			IRepoTransaction repoTransaction)
		{
			DatabaseEngineConstant databaseEngine = DatabaseOption.DatabaseEngine;
			var messageRepo = MessageRepoFactory.GetMessageRepository(databaseEngine,
				DatabaseOption.DbConnectionString, repoTransaction);
			messageRepo.InsertMessage(message);
			LogInfo("Message persisting was successful");
		}

		private void ProcessMessageDispatch(IMessageContract messageContract,
			Message message, IRepoTransaction repoTransaction)
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

		/*private void PersistMessageToMongoDbService(Message contract)
		 {
			 try
			 {
				 RabbitMqProducerClass rabbitMqProducer = new RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
					 QueueTypeConstant.MongoDbPersistentUserService);
				 rabbitMqProducer.ExecuteMessageQueueing(contract);
				 WriteInfoLog("Queueing contract to Message-Queue was successful.");
			 }
			 catch (Exception exception)
			 {
				 MessageQueueErrorContract error = new MessageQueueErrorContract()
				 {
					 Message = "Error encountered when trying to queue to contract queue.",
					 ExceptionMessage = exception.Message
				 };
				 WriteErrorLog("Error encountered when queueing contract to Message-Queue.", exception);
				 //throw new FaultException<MessageQueueErrorContract>(error);
			 }
		 }*/
	}
}