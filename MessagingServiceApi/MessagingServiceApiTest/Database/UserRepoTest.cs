using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace WMessageServiceApiTest.Database
{
	[TestClass]
	public class UserRepoTest
	{
		[TestMethod]
		public void InsertUser_MsSql_ADOdotnet()
		{
			User user = GetUser();
			AdvancedUser advancedUser = GetAdvanceUser();

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			userRepo.InsertUser(user);
			userRepo.InsertUser(advancedUser);

			if (user.Id == 0 ||
				 advancedUser.Id == 0)
			{
				Assert.Fail("Items did not persist");
			}
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

		private AdvancedUser GetAdvanceUser()
		{
			Random randomNumber = new Random();
			string firstName = "UnitTestOne" + randomNumber.Next(1000000);
			string secondName = "UnitTestMsql" + randomNumber.Next(1000000);

			AdvancedUser advancedUser = new AdvancedUser()
			{
				FirstName = "advance " + firstName,
				Surname = "advance " + secondName,
				DOB = DateTime.Now,
				Gender = UserDataConstants.Male,
				UserName = "advance " + firstName + "_" + secondName,
				Password = "password" + firstName + "_" + secondName,
				AdvanceStartDatetime = DateTime.Now,
				AdvanceEndDatetime = DateTime.Now.AddDays(50)
			};

			return advancedUser;
		}

		[TestMethod]
		public void UpdateUser_Mssql_ADOdotnet()
		{
			User user = GetUser();
			AdvancedUser advancedUser = GetAdvanceUser();

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			userRepo.InsertUser(user);
			userRepo.InsertUser(advancedUser);

			if (user.Id == 0 ||
				 advancedUser.Id == 0)
			{
				Assert.Fail("Items did not persist");
			}

			user.EmailAddress = user.EmailAddress + "_updatedtest";
			advancedUser.EmailAddress = advancedUser.EmailAddress + "_updatedtest";

			userRepo.UpdateUser(user);
			userRepo.UpdateUser(advancedUser);

			User user1 = userRepo.GetUserMatchingId(user.Id);
			AdvancedUser advancedUser1 = userRepo.GetUserMatchingId(advancedUser.Id) as AdvancedUser;

			Assert.AreEqual(user.EmailAddress, user1.EmailAddress);
			Assert.AreEqual(advancedUser.EmailAddress, advancedUser1.EmailAddress);
		}

		[TestMethod]
		public void DeleteUser_Mssql_ADOdotnet()
		{
			User user = GetUser();
			AdvancedUser advancedUser = GetAdvanceUser();

			user.EmailAddress = user.EmailAddress + "_usershouldn'texist";
			advancedUser.EmailAddress = advancedUser.EmailAddress + "_usershouldn'texist";

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			userRepo.InsertUser(user);
			userRepo.InsertUser(advancedUser);

			if (user.Id == 0 ||
				 advancedUser.Id == 0)
			{
				Assert.Fail("Items did not persist");
			}

			User user1 = userRepo.GetUserMatchingId(user.Id);
			AdvancedUser advancedUser1 = userRepo.GetUserMatchingId(advancedUser.Id) as AdvancedUser;

			userRepo.DeleteUser(user1);
			userRepo.DeleteUser(advancedUser1);

			User user2 = userRepo.GetUserMatchingId(user.Id);
			AdvancedUser advancedUser2 = userRepo.GetUserMatchingId(advancedUser.Id) as AdvancedUser;

			Assert.IsNull(user2);
			Assert.IsNull(advancedUser2);
		}

		[TestMethod]
		public void RetrieveUser_Mssql_ADOdotnet()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<User> users = userRepo.GetAllUsers();
			Assert.IsNotNull(users);
		}

		[TestMethod]
		public void RetrieveUserMatchingId_Mssql_ADOdotnet()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingId(1);
			Assert.IsNotNull(user);
		}

		[TestMethod]
		public void RetrieveUserMatchingUserNamePassword_Mssql_ADOdotnet()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword("Dada", "qwerty");
			Assert.IsNotNull(user);
		}

		[TestMethod]
		public void RetrieveUserMatchingUsername_Mssql_ADOdotnet()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername("Dada");
			Assert.IsNotNull(user);
		}

		[TestMethod]
		public void UpdateAllUserEmailAddress_Msqql_ADODOTNET()
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseEngineConstant.MsSqlAdoDotNet,
				 DatabaseOption.DbConnectionString);
			List<User> users = userRepo.GetAllUsers();
			Assert.IsNotNull(users);
			foreach (User user in users)
			{
				if (user.EmailAddress != null &&
					 user.EmailAddress.Contains("@wcfmsgproj.com"))
				{
					continue;
				}
				user.EmailAddress = string.Format("{0}@wcfmsgproj.com", user.UserName);
				userRepo.UpdateUser(user);
			}
		}

		/*[TestMethod]
	 public void DoesEntityExistMatchingId_CustomEngine_Mssql()
	 {
		 IUserRepository userRepo = UserRepoFactory.C(DatabaseEngineOptionConstant.Mssql_CustomEngine);
		 bool? user = userRepo.DoesEntityExistMatchingId(1);
		 Assert.IsNotNull(user);
		 Assert.AreEqual(true, user);
	 }*/
	}
}