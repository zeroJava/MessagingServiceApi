﻿using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using System;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Logging;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;
using WMessageServiceApi.Messaging.DataEnumerations;
using Transaction = MessageDbLib.DbRepositoryFactories.RepoTransactionFactory;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class CreateMessageFacade
	{
		public MessageRequestTokenContract CreateMessage(IMessageContract message)
		{
			ValidateAccessToken(message.AccessToken);

			if (message.EmailAccounts == null || message.EmailAccounts.Count <= 0)
			{
				throw new InvalidOperationException("Message contract does not have" +
					" ant emails attahed.");
			}

			LogInfo(string.Format("Going to create message. Message content\n{0}",
				message.Message));
			Message newMessage = CreateNewMessage(message);
			ProcessNewMessage(message, newMessage);
			//PersistMessageToMongoDbService(newMessage);

			return CreateMessageStateTokenContract(MessageReceivedState.AcknowledgedRequest,
				"Message was successfully acknowledged and persisted in our system.");
		}

		private void ValidateAccessToken(string encryptedToken)
		{
			string option = AccessTokenValidatorFactory.ACCESS_TOKEN_WCF;
			IAccessTokenValidator tokenValidator = AccessTokenValidatorFactory.GetAccessTokenValidator(option);
			TokenValidationResult result = tokenValidator.IsTokenValid(encryptedToken);

			if (!result.IsValidationSuccess)
			{
				throw new TokenValidationException(result.Message, result.Status);
			}
		}

		private MessageRequestTokenContract CreateMessageStateTokenContract(
			MessageReceivedState recievedState,
			string message)
		{
			return new MessageRequestTokenContract
			{
				MessageRecievedState = recievedState,
				Message = message
			};
		}

		private Message CreateNewMessage(IMessageContract messageContract)
		{
			User user = RetrieveUser(messageContract.UserName);
			Message newMessage = new Message
			{
				MessageText = messageContract.Message,
				SenderId = user.Id,
				SenderEmailAddress = user.EmailAddress,
				MessageCreated = messageContract.MessageCreated
			};
			return newMessage;
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

		private void ProcessNewMessage(IMessageContract messageContract,
			Message message)
		{
			using (IRepoTransaction repoTransaction = Transaction.GetRepoTransaction(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString))
			{
				try
				{
					repoTransaction.BeginTransaction();
					PersistMessage(message, repoTransaction);
					ProcessMessageDispatch(messageContract, message, repoTransaction);
					repoTransaction.Commit();
				}
				catch (Exception exception)
				{
					var logMessage = $"Unabled to process new message request\n" +
						$"{exception}";
					LogError("Unable to process new message request");
					repoTransaction.Callback();
					throw;
				}
			}
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
			Message message,
			IRepoTransaction repoTransaction)
		{
			foreach (var emailAddress in messageContract.EmailAccounts)
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

		/*private void PersistMessageToMongoDbService(Message message)
		 {
			 try
			 {
				 RabbitMqProducerClass rabbitMqProducer = new RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
					 QueueTypeConstant.MongoDbPersistentUserService);
				 rabbitMqProducer.ExecuteMessageQueueing(message);
				 WriteInfoLog("Queueing message to Message-Queue was successful.");
			 }
			 catch (Exception exception)
			 {
				 MessageQueueErrorContract error = new MessageQueueErrorContract()
				 {
					 Message = "Error encountered when trying to queue to message queue.",
					 ExceptionMessage = exception.Message
				 };
				 WriteErrorLog("Error encountered when queueing message to Message-Queue.", exception);
				 //throw new FaultException<MessageQueueErrorContract>(error);
			 }
		 }*/

		private void LogError(string message)
		{
			AppLog.LogError(message);
		}

		private void LogInfo(string message)
		{
			AppLog.LogInfo(message + ".");
		}
	}
}