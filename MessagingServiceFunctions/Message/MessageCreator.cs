using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;
using DbMessage = MessageDbCore.RepoEntity.Message;
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
			userRepository = UserRepoFactory.GetUserRepository(engine,
				connectionString);
		}

		public MessageCreator(DatabaseEngineConstant engine,
			string connectionString) :
			base(engine, connectionString)
		{
			userRepository = UserRepoFactory.GetUserRepository(engine,
				connectionString);
		}

		public MessageCreator(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
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
		}

		private void ProcessTransaction(DbMessage msg, IMessageRequest request)
		{
			using (var transaction =
				Transaction.GetRepoTransaction(engine, connectionString))
			{
				try
				{
					transaction.BeginTransaction();
					SaveMessage(msg, transaction);
					CreateDispatch(request, msg, transaction);
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

		private void CreateDispatch(IMessageRequest messageContract,
			DbMessage message, IRepoTransaction repoTransaction)
		{
			foreach (string emailAddress in messageContract.EmailAccounts)
			{
				SaveDispatch(new MessageDispatch
				{
					EmailAddress = emailAddress,
					MessageId = message.Id,
					MessageReceived = false
				}, repoTransaction);
			}
		}

		private void SaveDispatch(MessageDispatch messageDispatch,
			IRepoTransaction repoTransaction)
		{
			var dispatchRepo = GetDispatchRepo(repoTransaction);
			dispatchRepo.InsertDispatch(messageDispatch);
		}

		private IMessageDispatchRepository GetDispatchRepo(IRepoTransaction repoTransaction)
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(engine,
				connectionString, repoTransaction);
		}
	}
}