using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.Login;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WMessageServiceApiTest.ServicesBusinessLogic
{
	[TestClass]
	public class LoginBLTest
	{
		[TestMethod]
		public void ExecuteEncryptedLoginIn_BL_Test()
		{
			LoginService loginLogic = new LoginService();
			LoginToken loginToken = loginLogic.ExecuteEncryptedLoginIn("JonathanTest", "Test1");
			if (loginToken == null ||
				 loginToken.LoginSuccessful == false)
			{
				Assert.Fail();
			}
		}
	}
}