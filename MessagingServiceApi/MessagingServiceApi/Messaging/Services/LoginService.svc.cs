using MessagingServiceInterfaces.Contracts.Login;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
	public class LoginService : ILoginService
	{
		public LoginToken ExecuteEncryptedLoginIn(string encryptedUser,
			string encryptedPassword)
		{
			ServiceLogics.LoginService serviceHelper = new ServiceLogics.LoginService();
			return serviceHelper.ExecuteEncryptedLoginIn(encryptedUser,
				encryptedPassword);
		}
	}
}
