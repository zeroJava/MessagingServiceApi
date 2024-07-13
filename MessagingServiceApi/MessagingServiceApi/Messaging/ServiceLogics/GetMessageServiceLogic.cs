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
				LogMethodInvoked(nameof(GetMessagesSentToUser));
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
			var engine = Dboption.DatabaseEngine;
			var connectionString = Dboption.DbConnectionString;
			var messageRepository =
				MessageRepoFactory.GetMessageRepository(engine,	connectionString);
			var dispatchRepository =
				MessageDispatchRepoFactory.GetDispatchRepository(engine,
				connectionString);
			var userRepository =
				UserRepoFactory.GetUserRepository(engine, connectionString);
			return new MessageRetriever(messageRepository,
				dispatchRepository, userRepository);
		}

		public List<PostedMessageInfo> GetConveration(string token,
			IRetrieveMessageRequest messageRequest)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(GetConveration));
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