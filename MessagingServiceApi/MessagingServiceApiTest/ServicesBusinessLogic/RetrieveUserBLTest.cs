using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class RetrieveUserBLTest
	{
		[TestMethod]
		public void GetAllUsers_Test_BL()
		{
			UserRetriever userLogic = new UserRetriever();
			List<UserInfo> users = userLogic.GetAllUsers();
			if (users == null)
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void GetUserMatchingUsernamePassword_Test_BL()
		{
			UserRetriever userLogic = new UserRetriever();
			UserInfo user = userLogic.GetUserMatchingUsername("BallyTest");
			if (user == null)
			{
				Assert.Fail();
			}
		}
	}
}
