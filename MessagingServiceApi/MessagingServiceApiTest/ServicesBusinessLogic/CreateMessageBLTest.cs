using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceFunctions.Message;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class CreateMessageBLTest
	{
		[TestMethod]
		public void CreateMessage_Test()
		{
			MessageRequest message = new MessageRequest()
			{
				UserName = GetSenderUserName(),
				EmailAccounts = new List<string>() { GetReceiverEmail() },
				Message = "Test the create message business logic",
				MessageCreated = DateTime.Now
			};
			MessageCreator essageCreator = new MessageCreator();
			MessageRequestToken requestToken = essageCreator.Create(message);
			if (requestToken.MessageRecievedState == MessageReceivedState.FailedToProcess)
			{
				Assert.Fail();
			}
		}

		private string GetSenderUserName()
		{
			string username = "BallyTest";

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(username);
			if (user == null)
			{
				user = new User()
				{
					FirstName = "Bally",
					Surname = "Test",
					UserName = username,
					Password = "test1",
					EmailAddress = "BallyTest@wcfmsgproj.com",
					DOB = new DateTime(1980, 2, 1),
					Gender = "Male"
				};
				userRepo.InsertUser(user);
			}
			return user.UserName;
		}

		private string GetReceiverEmail()
		{
			string username = "JonathanTest";

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(username);
			if (user == null)
			{
				user = new User()
				{
					FirstName = "Jonathan",
					Surname = "Test",
					UserName = username,
					Password = "test1",
					EmailAddress = "JonathanTest@wcfmsgproj.com",
					DOB = new DateTime(1980, 2, 1),
					Gender = "Male"
				};
				userRepo.InsertUser(user);
			}
			return user.UserName;
		}
	}
}