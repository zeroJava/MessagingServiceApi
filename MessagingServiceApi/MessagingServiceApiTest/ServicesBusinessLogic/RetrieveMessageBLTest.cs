using Cryptography;
using MessageDbCore.DbRepositoryInterfaces;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceFunctions.Message;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.IContracts.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using Dboption = MessageDbLib.Configurations.DatabaseOption;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class RetrieveMessageBLTest
	{
		private const string USERNAME = "JonathanTest";
		private const string SENDEREMAIL = "BallyTest@wcfmsgproj.com";
		private const string RECEIVEREMAIL = "JonathanTest@wcfmsgproj.com";

		[TestMethod]
		public void GetMessagesSentToUser_BL_Test()
		{
			string encrpptedUsername = SymmetricEncryption.Encrypt(USERNAME);
			IRetrieveMessageRequest messageRequest = new RetrieveMessageRequest
			{
				UserAccessToken = encrpptedUsername,
				ReceiverEmailAddress = RECEIVEREMAIL,
			};
			IMessageRepository messageRepository =
				MessageRepoFactory.GetMessageRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IMessageDispatchRepository dispatchRepository =
				MessageDispatchRepoFactory.GetDispatchRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IUserRepository userRepository =
				UserRepoFactory.GetUserRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			MessageRetriever retrieveMessageLogic = new MessageRetriever(messageRepository,
				dispatchRepository,
				userRepository);
			List<PostedMessageInfo> dispatchContracts = retrieveMessageLogic.GetMessagesSentToUser(messageRequest);
			if (dispatchContracts == null)
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void GetMessageDispatchesBetweenSenderReceiver_BL_Test()
		{
			string encrpptedUsername = SymmetricEncryption.Encrypt(USERNAME);
			IRetrieveMessageRequest messageRequest = new RetrieveMessageRequest
			{
				UserAccessToken = encrpptedUsername,
				SenderEmailAddress = SENDEREMAIL,
				ReceiverEmailAddress = RECEIVEREMAIL,
				MessageIdThreshold = 500000,
				NumberOfMessages = 500,
			};
			IMessageRepository messageRepository =
				MessageRepoFactory.GetMessageRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IMessageDispatchRepository dispatchRepository =
				MessageDispatchRepoFactory.GetDispatchRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			IUserRepository userRepository =
				UserRepoFactory.GetUserRepository(Dboption.DatabaseEngine,
					Dboption.DbConnectionString);
			MessageRetriever retrieveMessageLogic = new MessageRetriever(messageRepository,
				dispatchRepository,
				userRepository);
			List<PostedMessageInfo> dispatchContracts = retrieveMessageLogic.GetConversation(messageRequest);
			if (dispatchContracts == null)
			{
				Assert.Fail();
			}
		}
	}
}