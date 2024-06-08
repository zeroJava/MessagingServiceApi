using MessageDbCore.DbRepositoryInterfaces;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.Message;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;

using Dboption = MessageDbLib.Configurations.DatabaseOption;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class GetMessageServiceLogic : LogicBase
	{
		public List<PostedMessageInfo> GetMessagesSentToUser(string token,
			IRetrieveMessageRequest messageRequest)
		{
			try
			{
				ValidateToken(token);
				LogMethodICalled(nameof(GetMessagesSentToUser));
				MessageRetriever messageRetriever = GetMessageRetriever();
				return messageRetriever.GetMessagesSentToUser(messageRequest);
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(GetMessagesSentToUser));
				throw processError.Invoke(exception.Message);
			}
		}

		private MessageRetriever GetMessageRetriever()
		{
			IMessageRepository messageRepository =
				MessageRepoFactory.GetMessageRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IMessageDispatchRepository dispatchRepository =
				MessageDispatchRepoFactory.GetDispatchRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IUserRepository userRepository =
				UserRepoFactory.GetUserRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			return new MessageRetriever(messageRepository,
				dispatchRepository,
				userRepository);
		}

		public List<PostedMessageInfo> GetConveration(string token,
			IRetrieveMessageRequest messageRequest)
		{
			try
			{
				ValidateToken(token);
				LogMethodICalled(nameof(GetConveration));
				MessageRetriever messageRetriever = GetMessageRetriever();
				return messageRetriever.GetConversation(messageRequest);
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(GetConveration));
				throw processError.Invoke(exception.Message);
			}
		}
	}
}