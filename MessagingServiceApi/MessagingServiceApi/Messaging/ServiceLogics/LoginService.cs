using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Contracts.Errors;
using MessagingServiceInterfaces.Contracts.Login;
using System;
using System.ServiceModel;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class LoginService
	{
		public LoginToken ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword)
		{
			try
			{
				return Login(encryptedUser, encryptedPassword);
			}
			catch (Exception exception)
			{
				throw new FaultException<LoginError>(new LoginError
				{
					Message = exception.Message,
				});
			}
		}

		private static LoginToken Login(string encryptedUser, string encryptedPassword)
		{
			string decryptedUsername = encryptedUser;
			string decryptedPassword = encryptedPassword;

			IUserRepository userRepo =
				UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine,
				DatabaseOption.DbConnectionString);
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