using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;
using DbMessage = MessageDbCore.RepoEntity.Message;
using Dboption = MessageDbLib.Configurations.DatabaseOption;
using DbUser = MessageDbCore.RepoEntity.User;
using Transaction = MessageDbLib.DbRepositoryFactories.RepoTransactionFactory;

namespace MessagingServiceFunctions.Message
{
	public class MessageCreator : FunctionBase
	{
		private static readonly MessageRequestToken MessageSuccess =
			new MessageRequestToken
			{
				MessageRecievedState = MessageReceivedState.Successful,
				Message = "Message was successfully acknowledged and stored"
			};

		private readonly IUserRepository userRepository;

		public MessageCreator()
		{
			userRepository =
				UserRepoFactory.GetUserRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
		}

		public MessageRequestToken Create(IMessageRequest request)
		{
			Validate(request);
			DbUser user = GetUser(request.UserName);
			CreateMessage(request, user);
			return MessageSuccess;
		}

		private void Validate(IMessageRequest request)
		{
			if (request.EmailAccounts?.Count > 0)
			{
				return;
			}
			throw new InvalidOperationException(
				"Message request does not have any emails attached.");
		}

		private void CreateMessage(IMessageRequest request, DbUser user)
		{
			ProcessTransaction(new DbMessage
			{
				MessageText = request.Message,
				SenderId = user.Id,
				SenderEmailAddress = user.EmailAddress,
				MessageCreated = request.MessageCreated
			}, request);
			//PersistMessageToMongoDbService(message);
		}

		private void ProcessTransaction(DbMessage msg, IMessageRequest request)
		{
			using (var transaction =
				Transaction.GetRepoTransaction(Dboption.DatabaseEngine,
				Dboption.DbConnectionString))
			{
				try
				{
					transaction.BeginTransaction();
					SaveMessage(msg, transaction);
					var dispatches = CreateDispatch(request, msg, transaction);
					foreach (var dispatch in dispatches)
					{
						SaveDispatch(dispatch, transaction);
					}
					transaction.Commit();
				}
				catch
				{
					transaction.Callback();
					throw;
				}
			}
		}

		private DbUser GetUser(string userName)
		{
			DbUser user = userRepository.GetUserMatchingUsername(userName);
			return user ??
				throw new InvalidOperationException("Sender could not be found in" +
				" our current repo");
		}

		private void SaveMessage(DbMessage message,
			IRepoTransaction repoTransaction)
		{
			var messageRepo = MessageRepoFactory.GetMessageRepository(engine,
				connectionString, repoTransaction);
			messageRepo.InsertMessage(message);
		}

		private List<MessageDispatch> CreateDispatch(IMessageRequest messageContract,
			DbMessage message, IRepoTransaction repoTransaction)
		{
			List<MessageDispatch> messageDispatches = new List<MessageDispatch>();
			foreach (string emailAddress in messageContract.EmailAccounts)
			{
				SaveDispatch(new MessageDispatch
				{
					EmailAddress = emailAddress,
					MessageId = message.Id,
					MessageReceived = false
				}, repoTransaction);
			}
			return messageDispatches;
		}

		private void SaveDispatch(MessageDispatch messageDispatch,
			IRepoTransaction repoTransaction)
		{
			var dispatchRepo = GetDispatchRepo(repoTransaction);
			dispatchRepo.InsertDispatch(messageDispatch);
		}

		private static IMessageDispatchRepository GetDispatchRepo(IRepoTransaction repoTransaction)
		{
			var engine = Dboption.DatabaseEngine;
			var connectionString = Dboption.DbConnectionString;
			return MessageDispatchRepoFactory.GetDispatchRepository(engine,
				connectionString, repoTransaction);
		}

		/*private void PersistMessageToMongoDbService(Message request)
		 {
			 try
			 {
				 RabbitMqProducerClass rabbitMqProducer = new RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
					 QueueTypeConstant.MongoDbPersistentUserService);
				 rabbitMqProducer.ExecuteMessageQueueing(request);
				 LogInfo("Queueing request to Message-Queue was successful.");
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