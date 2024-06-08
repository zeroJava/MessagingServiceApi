using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Message;
using MessagingServiceInterfaces.Contracts.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class CreateUserBLTest
	{
		[TestMethod]
		public void CreateUser_BL_Test()
		{
			Random random = new Random();
			int randomNumber = random.Next(0, 999999999);
			NewUserData user = new NewUserData()
			{
				FirstName = "Bally" + randomNumber,
				Surname = "Test" + randomNumber,
				UserName = "BallyTest_" + randomNumber,
				Password = "test1",
				EmailAddress = "BallyTest_" + randomNumber + "@testdomain.com",
				Dob = new DateTime(1980, 2, 1),
				Gender = "Male"
			};
			UserCreator userCreator = new UserCreator();
			userCreator.CreateNewUser(user);
		}

		[TestMethod]
		public void CreateAdvanceUser_BL_Test()
		{
			Random random = new Random();
			int randomNumber = random.Next(0, 999999999);
			NewAdvancedUserData user = new NewAdvancedUserData()
			{
				FirstName = "BallyAdvance" + randomNumber,
				Surname = "TestAdvance" + randomNumber,
				UserName = "BallyTestAdvance_" + randomNumber,
				Password = "test1",
				EmailAddress = "BallyTestAdvance_" + randomNumber + "@testdomain.com",
				Dob = new DateTime(1980, 2, 1),
				Gender = "Male",
				AdvanceStartDatetime = DateTime.Now,
				AdvanceEndDatetime = DateTime.Now.AddYears(1)
			};
			UserCreator userCreator = new UserCreator();
			userCreator.CreateNewUser(user);
		}
	}
}
