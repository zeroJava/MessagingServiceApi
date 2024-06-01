using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using stringCheck = WMessageServiceApi.Extensions.StringExtension;

namespace WMessageServiceApi.Messaging.Facades
{
	public class UpdateUserDataServiceFacade
	{
		public void UpdateUser(INewUserDataContract userContract)
		{
			if (userContract == null ||
				 userContract.UserName == null ||
				 userContract.UserName == string.Empty)
			{
				string message = userContract == null ?
					"The user contract, recieved is null." : "Username is empty";
				ThrowEntityErrorMessage(message, StatusList.ProcessError);
			}
			User user = GetUserEntityObject(userContract.UserName);
			if (user == null)
			{
				throw new InvalidOperationException($"Could not find user with" +
					$" username: {userContract.UserName}");
			}
			UpdateUserEntityObject(user, userContract);
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			userRepo.UpdateUser(user);
		}

		private User GetUserEntityObject(string userName)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(
				DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
			return userRepo.GetUserMatchingUsername(userName);
		}

		private void UpdateUserEntityObject(User userEntityObject,
			INewUserDataContract userContract)
		{
			if (stringCheck.IsNotNullEmpty(userContract.FirstName))
			{
				userEntityObject.FirstName = userContract.FirstName;
			}
			if (stringCheck.IsNotNullEmpty(userContract.Surname))
			{
				userEntityObject.Surname = userContract.Surname;
			}
			if (stringCheck.IsNotNullEmpty(userContract.EmailAddress))
			{
				userEntityObject.EmailAddress = userContract.EmailAddress;
			}
		}

		private void ThrowEntityErrorMessage(string message, int status)
		{
			ErrorContract error = new ErrorContract(message, status);
			throw new FaultException<ErrorContract>(error);
		}
	}
}