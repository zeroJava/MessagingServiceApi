using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.LoginContracts;

namespace WMessageServiceApi.Messaging.ServiceHelpers
{
	public class LoginServiceHelper
	{
		public LoginToken ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword)
		{
			try
			{
				return Login(encryptedUser, encryptedPassword);
			}
			catch (Exception exception)
			{
				throw new FaultException<LoginErrorContract>(new LoginErrorContract
				{
					Message = exception.Message,
				});
			}
		}

		public static LoginToken Login(string encryptedUser, string encryptedPassword)
		{
			string decryptedUsername = encryptedUser;
			string decryptedPassword = encryptedPassword;

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsername(decryptedUsername);

			LoginToken loginTokenContract = new LoginToken();
			if (user != null)
			{
				loginTokenContract.LoginSuccessful = true;
				loginTokenContract.UserName = user.UserName;
				loginTokenContract.UserEmailAddress = user.EmailAddress;
			}
			return loginTokenContract;
		}
	}
}