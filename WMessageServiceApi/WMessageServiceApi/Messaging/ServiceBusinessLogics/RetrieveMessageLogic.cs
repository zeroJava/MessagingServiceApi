using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class RetrieveMessageLogic : BaseLogic
	{
		public List<MessageDispatchInfo> GetMessagesSentToUser(
			IRetrieveMessageRequest messageRequest)
		{
			ValidateAccessToken(messageRequest.UserAccessToken);
			CheckRequestContent(messageRequest);

			string username = messageRequest.Username;
			LogInfo($"Getting messages sent to user: {username};");
			User user = GetUserMatchingUsername(username) ??
				throw new ApplicationException("Could not find a matching " +
				"Username.");

			return MessagesSentToUser(user.Id, messageRequest.ReceiverEmailAddress);
		}

		private void CheckRequestContent(IRetrieveMessageRequest messageRequest)
		{
			string username = messageRequest.Username;
			if (string.IsNullOrEmpty(username))
			{
				throw new ApplicationException("Username value passed is empty.");
			}
		}

		private User GetUserMatchingUsername(string username)
		{
			IUserRepository userRepo = GetUserRepository();
			User user = userRepo.GetUserMatchingUsername(username);
			return user;
		}

		private IUserRepository GetUserRepository()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(
				DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
			return userRepo;
		}

		private List<MessageDispatchInfo> MessagesSentToUser(long? userId,
			string receiverEmail)
		{
			IMessageDispatchRepository dispatchRepo = GetDispatchRepository();
			List<MessageDispatch> dispatches = 
				dispatchRepo.GetDispatchesNotReceived(receiverEmail);
			AssignMessagesToDispatch(dispatches);
			return GetDispatchInfo(dispatches, userId);
		}

		private long[] GetMessageIds(List<MessageDispatch> dispatches)
		{
			List<long> messageIds = new List<long>();
			for (int i = 0; i < dispatches?.Count; i++)
			{
				if (dispatches[i]?.MessageId == null)
				{
					continue;
				}
				long messageid = dispatches[i].MessageId.Value;
				if (!messageIds.Contains(messageid))
				{
					messageIds.Add(messageid);
				}
			}
			return messageIds.ToArray();
		}

		private IMessageDispatchRepository GetDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(
				DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
		}

		private void AssignMessagesToDispatch(List<MessageDispatch> dispatches)
		{
			long[] messageIds = GetMessageIds(dispatches);
			List<Message> messages = null;

			if (messageIds?.Length > 0)
			{
				IMessageRepository messageRepo = GetMessageRepository();
				messages = messageRepo.GetMessages(messageIds);
			}

			if (messages != null)
			{
				foreach (MessageDispatch dispatch in dispatches)
				{
					Message message = messages
						.FirstOrDefault(m => m.Id == dispatch.MessageId);
					if (message != null)
					{
						dispatch.Message = message;
					}
				}
			}
		}

		private IMessageRepository GetMessageRepository()
		{
			return MessageRepoFactory.GetMessageRepository(
				DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
		}

		private List<MessageDispatchInfo> GetDispatchInfo(
			List<MessageDispatch> messageDispatches,
			long? userId)
		{
			try
			{
				var msgDispatchinfos = new List<MessageDispatchInfo>();
				foreach (MessageDispatch dispatch in messageDispatches)
				{
					bool isSender = dispatch.Message.SenderId == userId;
					Message message = dispatch.Message;
					var dispatchInfo = new MessageDispatchInfo
					{
						SenderName = message.SenderEmailAddress,
						ReceiverName = dispatch.EmailAddress,
						MessageSentDate = message.MessageCreated,
						MessageReceivedDate = dispatch.MessageReceivedTime,
						MessageReceived = dispatch.MessageReceived,
						MessageContent = message.MessageText,
						SenderCurrentUser = isSender,
						DispatchId = dispatch.Id,
						MessageId = message.Id
					};
					msgDispatchinfos.Add(dispatchInfo);
				}
				return msgDispatchinfos;
			}
			catch (Exception exception)
			{
				LogError("Error encountered when executing Convert-Message-Dispatch" +
					"-To-Contract.",
					exception);
				return null;
			}
		}

		public List<MessageDispatchInfo> GetMsgDispatchesBetweenSenderReceiver(
			IRetrieveMessageRequest messageRequest)
		{
			ValidateAccessToken(messageRequest.UserAccessToken);

			string username = messageRequest.Username;
			if (string.IsNullOrEmpty(username))
			{
				throw new ApplicationException("Username value passed is empty.");
			}
			User user = GetUserMatchingUsername(username) ??
				throw new ApplicationException($"Could not find user matching " +
				$"{username}");
			List<MessageDispatchInfo> dispatchInfos =
				GetDispatchesBetweenSenderReceiver(messageRequest, user);

			return dispatchInfos;
		}

		private List<MessageDispatchInfo> GetDispatchesBetweenSenderReceiver(
			IRetrieveMessageRequest messageRequest,
			User user)
		{
			LogInfo($"Getting messages between " +
				$"{messageRequest.SenderEmailAddress} and" +
				$"{messageRequest.ReceiverEmailAddress}");

			IMessageDispatchRepository dispatchRepo = GetDispatchRepository();
			List<MessageDispatch> dispatches =
				dispatchRepo.GetDispatchesSenderReceiver(
					messageRequest.SenderEmailAddress,
					messageRequest.ReceiverEmailAddress,
					messageRequest.MessageIdThreshold,
					messageRequest.NumberOfMessages);
			List<MessageDispatchInfo> dispatchInfos = GetDispatchInfo(dispatches,
				user.Id);
			return dispatchInfos;
		}
	}
}