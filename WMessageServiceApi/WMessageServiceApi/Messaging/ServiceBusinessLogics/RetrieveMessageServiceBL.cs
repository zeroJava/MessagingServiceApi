using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Logging;
using WMessageServiceApi.Messaging.DataContracts.MessageContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class RetrieveMessageServiceBl
    {
        public List<MessageDispatchInfoContract> GetMessagesSentToUser(IRetrieveMessageRequest messageRequest)
        {
            ValidateAccessToken(messageRequest.UserAccessToken);

            string username = messageRequest.Username;
            if (string.IsNullOrEmpty(username))
            {
                throw new ApplicationException("Username value passed is empty.");
            }

            User user = GetUserMatchingUsername(username);
            if (user == null)
            {
                throw new ApplicationException("Could not find a matching Username.");
            }

            LogInfo(string.Format("Getting messages sent to user: {0};", username));
            return MessagesSentToUser(user.Id, messageRequest.ReceiverEmailAddress);
        }

        private void ValidateAccessToken(string userAccessToken)
        {
            string option = AccessTokenValidatorFactory.ACCESS_TOKEN_WCF;

            if (string.IsNullOrEmpty(userAccessToken))
            {
                Console.WriteLine("This is a debug bypass, will be removed later.");
                return;
            }

            IAccessTokenValidator tokenValidator = AccessTokenValidatorFactory.GetAccessTokenValidator(option);
            TokenValidationResult result = tokenValidator.IsUserCredentialValid(userAccessToken);

            if (!result.IsValidationSuccess)
            {
                throw new TokenValidationException(result.Message, result.Status);
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
            IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine,
                DatabaseOption.DbConnectionString);
            return userRepo;
        }

        private List<MessageDispatchInfoContract> MessagesSentToUser(long? userId, string receiverEmailAddress)
        {
            List<MessageDispatch> messageDispathes = null;

            long[] messageIds = GetMessageIds(receiverEmailAddress, ref messageDispathes);
            if (messageDispathes == null || messageIds == null)
            {
                return null;
            }

            AssignMessagesToDispatch(messageDispathes, messageIds);
            List<MessageDispatchInfoContract> dispatchInfos = CreateDispatchInfoList(messageDispathes, userId);
            return dispatchInfos;
        }

        private long[] GetMessageIds(string receiverEmailAddress, ref List<MessageDispatch> messageDispatches)
        {
            IMessageDispatchRepository dispatchRepo = GetMessageDispatchRepository();
            messageDispatches = dispatchRepo.GetDispatchesNotReceivedMatchingEmail(receiverEmailAddress);

            if (messageDispatches == null ||
                messageDispatches.Count() == 0)
            {
                return null;
            }

            long[] messageids = messageDispatches.Where(mt => mt.MessageId != null)
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

        private void AssignMessagesToDispatch(List<MessageDispatch> messageDispatches, long[] messageIds)
        {
            IMessageRepository messageRepo = GetMessageRepository();
            List<Message> messages = messageRepo.GetAllMessages().Where(m => messageIds.Any(mi => mi == m.Id))
                .ToList();

            if (messages == null)
            {
                return;
            }

            foreach (MessageDispatch dispatch in messageDispatches)
            {
                Message message = messages.FirstOrDefault(m => m.Id == dispatch.MessageId) as Message;
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

        private List<MessageDispatchInfoContract> CreateDispatchInfoList(List<MessageDispatch> messageDispatches, long? userId)
        {
            try
            {
                List<MessageDispatchInfoContract> msgDispatchinfos = new List<MessageDispatchInfoContract>();
                foreach (MessageDispatch dispatch in messageDispatches)
                {
                    bool userIsSender = dispatch.Message.SenderId == userId;
                    MessageDispatchInfoContract dispatchInfo = CreateMessageDispatchInfoObj(dispatch, dispatch.Message, userIsSender);
                    msgDispatchinfos.Add(dispatchInfo);
                }
                return msgDispatchinfos;
            }
            catch (Exception exception)
            {
                LogError("Error encountered when executing Convert-Message-Dispatch-To-Contract.", exception);
                return null;
            }
        }

        private MessageDispatchInfoContract CreateMessageDispatchInfoObj(MessageDispatch messageDispatch, Message message,
            bool senderCurrentUser)
        {
            LogInfo("Creating message dispatch info contract.");

            string infomessage = string.Format("SenderName: {0}\n- ReceiverName: {1}" + "\n- SenderIsCurrentUser: {2}" +
                "\n- DispatchId: {3}" +
                "\n- MessageId: {4}", message.SenderEmailAddress,
                messageDispatch.EmailAddress,
                senderCurrentUser,
                messageDispatch.Id,
                message.Id);
            LogInfo(infomessage);

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

        public List<MessageDispatchInfoContract> GetMsgDispatchesBetweenSenderReceiver(IRetrieveMessageRequest messageRequest)
        {
            ValidateAccessToken(messageRequest.UserAccessToken);

            string username = messageRequest.Username;
            if (string.IsNullOrEmpty(username))
            {
                throw new ApplicationException("Username value passed is empty.");
            }

            User user = GetUserMatchingUsername(username);
            if (user == null)
            {
                throw new ApplicationException("Could not find a matching Username.");
            }

            string infotext = string.Format("Getting message messageDispatches between sender: {0} and receiver: {1}.",
                messageRequest.SenderEmailAddress,
                messageRequest.ReceiverEmailAddress);
            LogInfo(infotext);

            IMessageDispatchRepository dispatchRepo = GetMessageDispatchRepository();
            List<MessageDispatch> messageDispatches = dispatchRepo.GetDispatchesBetweenSenderReceiver(messageRequest.SenderEmailAddress,
                messageRequest.ReceiverEmailAddress,
                messageRequest.MessageIdThreshold,
                messageRequest.NumberOfMessages);
            List<MessageDispatchInfoContract> messageDispatchInfos = CreateDispatchInfoList(messageDispatches, user.Id);
            return messageDispatchInfos;
        }

        private void LogError(string message, Exception exception)
        {
            AppLog.LogError(message + "\n" + exception.ToString());
        }

        private void LogInfo(string message)
        {
            AppLog.LogInfo(message);
        }
    }
}