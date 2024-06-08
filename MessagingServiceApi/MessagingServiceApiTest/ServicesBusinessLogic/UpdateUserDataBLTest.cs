using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class UpdateUserDataBLTest
	{
		[TestMethod]
		public void UpdateUser_BL_Test()
		{
			User user = GetUser();

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			userRepo.InsertUser(user);

			if (user.Id == 0)
			{
				Assert.Fail("Items did not persist");
			}

			user.EmailAddress = user.EmailAddress + "_updatedtest";
			userRepo.UpdateUser(user);
			User user1 = userRepo.GetUserMatchingId(user.Id);

			UserUpdater updateUser = new UserUpdater();
			updateUser.UpdateUser(GetSETUser(user1));
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

		private NewUserData GetSETUser(User user)
		{
			return new NewUserData
			{
				UserName = user.UserName,
				Password = user.Password,
				FirstName = user.FirstName,
				Surname = user.Surname,
				Dob = user.DOB,
				Gender = user.Gender,
				EmailAddress = user.EmailAddress
			};
		}
	}
}
