using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.LoginContracts;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
	public class LoginService : ILoginService
	{
		public LoginToken ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword)
		{
			try
			{
				LoginLogic login = new LoginLogic();
				return login.ExecuteEncryptedLoginIn(encryptedUser, encryptedPassword);
			}
			catch (Exception exception)
			{
				throw new FaultException<LoginErrorContract>(new LoginErrorContract
				{
					Message = exception.Message,
				});
			}
		}

		private void ThrowErrorMessage<TContract>(string message) where TContract : IErrorsContract, new()
		{
			TContract error = new TContract
			{
				Message = message
			};
			throw new FaultException<TContract>(error);
		}
	}
}
