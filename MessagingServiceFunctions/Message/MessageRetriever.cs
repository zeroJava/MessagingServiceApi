using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using DbMessage = MessageDbCore.RepoEntity.Message;
using DbUser = MessageDbCore.RepoEntity.User;

namespace MessagingServiceFunctions.Message
{
	public class MessageRetriever : FunctionBase
	{
		private readonly IMessageRepository messageRepository;
		private readonly IMessageDispatchRepository dispatchRepository;
		private readonly IUserRepository userRepository;

		public MessageRetriever()
		{
			messageRepository = GetMessageRepository();
			dispatchRepository = GetDispatchRepository();
			userRepository = GetUserRepository();
		}

		public MessageRetriever(DatabaseEngineConstant engine,
			string connectionString) :
			base (engine, connectionString)
		{
			messageRepository = GetMessageRepository();
			dispatchRepository = GetDispatchRepository();
			userRepository = GetUserRepository();
		}

		public MessageRetriever(IMessageRepository messageRepository,
			IMessageDispatchRepository dispatchRepository,
			IUserRepository userRepository)
		{
			this.messageRepository = messageRepository;
			this.dispatchRepository = dispatchRepository;
			this.userRepository = userRepository;
		}

		private IMessageRepository GetMessageRepository()
		{
			return MessageRepoFactory.GetMessageRepository(engine,
				connectionString);
		}

		private IMessageDispatchRepository GetDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(engine,
				connectionString);
		}

		private IUserRepository GetUserRepository()
		{
			return UserRepoFactory.GetUserRepository(engine, connectionString);
		}

		public List<PostedMessageInfo> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
		{
			ValidateRequest(messageRequest);
			DbUser user = GetUser(messageRequest.Username) ??
				throw new ApplicationException("Could not find a matching Username.");
			long userid = user.Id;
			return GetMessagesSent(userid, messageRequest.ReceiverEmailAddress);
		}

		private void ValidateRequest(IRetrieveMessageRequest messageRequest)
		{
			string username = messageRequest.Username;
			if (string.IsNullOrEmpty(username))
				throw new ApplicationException("Username value passed is empty.");
		}

		private DbUser GetUser(string username)
		{
			DbUser user = userRepository.GetUserMatchingUsername(username);
			return user;
		}

		private List<PostedMessageInfo> GetMessagesSent(long? userId,
			string receiverEmail)
		{
			List<MessageDispatch> dispatches = dispatchRepository
				.GetDispatchesNotReceived(receiverEmail);
			List<long> messageIds = dispatches?.Where(x => x.MessageId != null)
				.Select(x => x.MessageId.Value)
				.Distinct()
				.ToList();
			List<DbMessage> messages = messageRepository
				.GetMessages(messageIds.ToArray());
			foreach (MessageDispatch dispatch in dispatches)
			{
				DbMessage message =
					messages.FirstOrDefault(m => m.Id == dispatch.MessageId);
				if (message != null)
					dispatch.Message = message;
			}
			return GetDispatchInfo(dispatches, userId);
		}

		private List<PostedMessageInfo> GetDispatchInfo(List<MessageDispatch> dispatches,
			long? userId)
		{
			var postedMessageInfo = new List<PostedMessageInfo>();
			foreach (MessageDispatch dispatch in dispatches)
			{
				bool isSender = dispatch.Message.SenderId == userId;
				DbMessage message = dispatch.Message;
				postedMessageInfo.Add(new PostedMessageInfo
				{
					MessageId = message.Id,
					DispatchId = dispatch.Id,
					MessageReceivedDate = dispatch.MessageReceivedTime,
					MessageSentDate = message.MessageCreated,
					SenderName = message.SenderEmailAddress,
					ReceiverName = dispatch.EmailAddress,
					MessageReceived = dispatch.MessageReceived,
					MessageContent = message.MessageText,
					SenderCurrentUser = isSender,
				});
			}
			return postedMessageInfo;
		}

		public List<PostedMessageInfo> GetConversation(IRetrieveMessageRequest messageRequest)
		{
			ValidateRequest(messageRequest);
			DbUser user = GetUser(messageRequest.Username) ??
				throw new ApplicationException("Could not find a matching Username.");
			List<PostedMessageInfo> postedMessages = GetDispathces(messageRequest,
				user);
			return postedMessages;
		}

		private List<PostedMessageInfo> GetDispathces(IRetrieveMessageRequest request,
			DbUser user)
		{
			List<MessageDispatch> dispatches = dispatchRepository
				.GetDispatchesSenderReceiver(request.SenderEmailAddress,
					request.ReceiverEmailAddress,
					request.MessageIdThreshold,
					request.NumberOfMessages);
			List<PostedMessageInfo> dispatchInfos = GetDispatchInfo(dispatches, user.Id);
			return dispatchInfos;
		}
	}
}