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
		public List<MessageDispatchInfoContract> GetMessagesSentToUser(
			IRetrieveMessageRequest messageRequest)
		{
			ValidateAccessToken(messageRequest.UserAccessToken);
			CheckRequestContent(messageRequest);

			string username = messageRequest.Username;
			LogInfo($"Getting messages sent to user: {username};");
			User user = GetUserMatchingUsername(username) ??
				throw new ApplicationException("Could not find a matching Username.");

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
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			return userRepo;
		}

		private List<MessageDispatchInfoContract> MessagesSentToUser(long? userId,
			string receiverEmailAddress)
		{
			List<MessageDispatch> messageDispathes = null;

			long[] messageIds = GetMessageIds(receiverEmailAddress, ref messageDispathes);
			if (messageDispathes == null || messageIds == null)
			{
				return null;
			}
			AssignMessagesToDispatch(messageDispathes, messageIds);
			List<MessageDispatchInfoContract> dispatchInfos = 
				GetDispatchInfo(messageDispathes, userId);

			return dispatchInfos;
		}

		private long[] GetMessageIds(string receiverEmailAddress,
			ref List<MessageDispatch> messageDispatches)
		{
			IMessageDispatchRepository dispatchRepo = GetMessageDispatchRepository();
			messageDispatches = dispatchRepo.GetDispatchesNotReceived(receiverEmailAddress);
			long[] messageids = messageDispatches?.Where(mt => mt.MessageId != null)
				.Select(mt => mt.MessageId.Value)
				.Distinct()
				.ToArray();
			return messageids;
		}

		private IMessageDispatchRepository GetMessageDispatchRepository()
		{
			return MessageDispatchRepoFactory.GetDispatchRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
		}

		private void AssignMessagesToDispatch(List<MessageDispatch> messageDispatches,
			long[] messageIds)
		{
			IMessageRepository messageRepo = GetMessageRepository();
			List<Message> messages = messageRepo.GetAllMessages()
				?.Where(m => messageIds.Any(mi => mi == m.Id))
				.ToList();
			if (messages == null)
			{
				return;
			}
			foreach (var dispatch in messageDispatches)
			{
				Message message = messages.FirstOrDefault(m => m.Id == dispatch.MessageId);
				if (message != null)
				{
					dispatch.Message = message;
				}
			}
			LogInfo("Completed assigning message to message-dispatch");
		}

		private IMessageRepository GetMessageRepository()
		{
			return MessageRepoFactory.GetMessageRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
		}

		private List<MessageDispatchInfoContract> GetDispatchInfo(
			List<MessageDispatch> messageDispatches, long? userId)
		{
			try
			{
				var msgDispatchinfos = new List<MessageDispatchInfoContract>();
				foreach (MessageDispatch dispatch in messageDispatches)
				{
					bool isSender = dispatch.Message.SenderId == userId;
					MessageDispatchInfoContract dispatchInfo = 
						CreateMessageDispatchInfoObj(dispatch, dispatch.Message,
						isSender);
					msgDispatchinfos.Add(dispatchInfo);
				}
				return msgDispatchinfos;
			}
			catch (Exception exception)
			{
				LogError("Error encountered when executing Convert-Message-Dispatch-To-Contract.",
					exception);
				return null;
			}
		}

		private MessageDispatchInfoContract CreateMessageDispatchInfoObj(
			MessageDispatch messageDispatch, Message message, bool senderCurrentUser)
		{
			LogInfo("Creating message dispatch info contract.");
			return new MessageDispatchInfoContract
			{
				SenderName = message.SenderEmailAddress,
				ReceiverName = messageDispatch.EmailAddress,
				MessageSentDate = message.MessageCreated,
				MessageReceivedDate = messageDispatch.MessageReceivedTime,
				MessageReceived = messageDispatch.MessageReceived,
				MessageContent = message.MessageText,
				SenderCurrentUser = senderCurrentUser,
				DispatchId = messageDispatch.Id,
				MessageId = message.Id
			};
		}

		public List<MessageDispatchInfoContract> GetMsgDispatchesBetweenSenderReceiver(
			IRetrieveMessageRequest messageRequest)
		{
			ValidateAccessToken(messageRequest.UserAccessToken);

			string username = messageRequest.Username;
			if (string.IsNullOrEmpty(username))
			{
				throw new ApplicationException("Username value passed is empty.");
			}
			User user = GetUserMatchingUsername(username) ??
				throw new ApplicationException($"Could not find user matching {username}");
			List<MessageDispatchInfoContract> dispatchInfos =
				GetDispatchesBetweenSenderReceiver(messageRequest, user);

			return dispatchInfos;
		}

		private List<MessageDispatchInfoContract> GetDispatchesBetweenSenderReceiver(
			IRetrieveMessageRequest messageRequest, User user)
		{
			LogInfo($"Getting messages between {messageRequest.SenderEmailAddress} and" +
				$" {messageRequest.ReceiverEmailAddress}");

			IMessageDispatchRepository dispatchRepo = GetMessageDispatchRepository();
			List<MessageDispatch> dispatches =
				dispatchRepo.GetDispatchesBetweenSenderReceiver(
					messageRequest.SenderEmailAddress,
					messageRequest.ReceiverEmailAddress,
					messageRequest.MessageIdThreshold,
					messageRequest.NumberOfMessages);
			List<MessageDispatchInfoContract> dispatchInfos = GetDispatchInfo(dispatches,
				user.Id);
			return dispatchInfos;
		}
	}
}