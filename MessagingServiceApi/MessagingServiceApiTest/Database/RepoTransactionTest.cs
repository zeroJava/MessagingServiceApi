using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbCore.Repositories;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepository.ADO.MsSql;
using MessageDbLib.DbRepositoryFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WMessageServiceApiTest.Database
{
	[TestClass]
	public class RepoTransactionTest
	{
		//[TestMethod]
		public void RepoTransactionTestOne()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword("UnitTestOne740918_UnitTestMsql417259",
				 "passwordUnitTestOne740918_UnitTestMsql417259");
			user.FirstName = "UnitTestOne740918_U6";

			using (IRepoTransaction repoTransaction = new RepoTransaction(DatabaseOption.DbConnectionString))
			{
				try
				{
					Message message = new Message()
					{
						MessageText = "Batch transaction test",
						MessageCreated = DateTime.Now,
						SenderId = 365046,
						SenderEmailAddress = @"Dada@wcfmsgproj.com",
					};
					repoTransaction.BeginTransaction();
					IMessageRepository messageRepo = GetMessageRepository(repoTransaction);
					userRepo = GetUserRepository(repoTransaction);
					userRepo.UpdateUser(user);
					messageRepo.InsertMessage(message);
					repoTransaction.Commit();
				}
				catch
				{
					repoTransaction.Callback();
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		public void RepoErrorTransactionTestOne()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword("UnitTestOne740918_UnitTestMsql417259", "passwordUnitTestOne740918_UnitTestMsql417259");
			user.FirstName = "UnitTestOne740918_U6_Error";

			using (IRepoTransaction repoTransaction = new RepoTransaction(DatabaseOption.DbConnectionString))
			{
				try
				{
					PictureMessage message = new PictureMessage()
					{
						MessageText = "Batch transaction test",
						MessageCreated = DateTime.Now,
						SenderId = 365046,
						SenderEmailAddress = @"Dada@wcfmsgproj.com",
						MediaFileType = "ThisIsALongFileTypeSoTheInserCanFailDuringTransactionProcessing",
					};
					repoTransaction.BeginTransaction();

					IMessageRepository messageRepo = GetMessageRepository(repoTransaction);
					userRepo = GetUserRepository(repoTransaction);

					userRepo.UpdateUser(user);
					messageRepo.InsertMessage(message);

					repoTransaction.Commit();
				}
				catch
				{
					repoTransaction.Callback();
				}
			}
		}

		private IMessageRepository GetMessageRepository(IRepoTransaction repoTransaction)
		{
			return MessageRepoFactory.GetMessageRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString,
				 repoTransaction);
		}

		private IUserRepository GetUserRepository(IRepoTransaction repoTransaction)
		{
			return UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString,
				 repoTransaction);
		}

		private User GetUser()
		{
			Random randomNumber = new Random();
			string firstName = "UnitTestOne" + randomNumber.Next(1000000);
			string secondName = "UnitTestMsql" + randomNumber.Next(1000000);

			User user = new User()
			{
				FirstName = firstName,
				Surname = secondName,
				DOB = DateTime.Now,
				Gender = UserDataConstants.Male,
				UserName = firstName + "_" + secondName,
				Password = "password" + firstName + "_" + secondName
			};

			return user;
		}

		//[TestMethod]
		public void RepoDisbaledTransactionTest()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword("UnitTestOne740918_UnitTestMsql417259", "passwordUnitTestOne740918_UnitTestMsql417259");
			user.FirstName = "UnitTestOne740918_U2";

			userRepo.UpdateUser(user);
		}
	}
}