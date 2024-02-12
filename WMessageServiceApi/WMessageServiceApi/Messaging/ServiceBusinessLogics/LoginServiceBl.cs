using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using WMessageServiceApi.Messaging.DataContracts.LoginContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public class LoginServiceBl : BaseBusinessLayer
	{
		public LoginTokenContract ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword)
		{
			string decryptedUsername = encryptedUser;
			string decryptedPassword = encryptedPassword;

			IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			User user = userRepo.GetUserMatchingUsernameAndPassword(decryptedUsername, decryptedPassword);

			LoginTokenContract loginTokenContract = new LoginTokenContract();
			if (user != null)
			{
				loginTokenContract.LoginSuccessful = true;
				loginTokenContract.UserName = user.UserName;
				loginTokenContract.UserEmailAddress = user.EmailAddress;
				//loginTokenContract.User = user;
			}
			/*if (retrieveUsers.EntityExistMatchingUsernameAndPassword(decryptedUsername, decryptedPassword) == true)
				{
					return true;
				}*/
			return loginTokenContract;
		}
	}
}