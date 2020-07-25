using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.LoginContracts;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;

namespace WMessageServiceApi.Messaging.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LoginService" in code, svc and config file together.
	// NOTE: In order to launch WCF Test Client for testing this service, please select LoginService.svc or LoginService.svc.cs at the Solution Explorer and start debugging.
	public class LoginService : ILoginService
	{
		public LoginTokenContract ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword)
		{
			try
			{
				LoginServiceBL loginBL = new LoginServiceBL();
				return loginBL.ExecuteEncryptedLoginIn(encryptedUser, encryptedPassword);
			}
			catch (Exception exception)
			{
				ThrowErrorMessage<LoginErrorContract>(exception.Message);
				return null;
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
