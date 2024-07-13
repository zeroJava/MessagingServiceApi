using MessageDbCore.DbRepositoryInterfaces;
using MessageDbLib.Constants;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.IContracts.User;
using System;

using DbUser = MessageDbCore.RepoEntity.User;
using stringCheck = MessagingServiceApi.Extensions.StringExtension;

namespace MessagingServiceFunctions.User
{
	public class UserUpdater : FunctionBase
	{
		private readonly IUserRepository userRepository;

		public UserUpdater()
		{
			userRepository = UserRepoFactory.GetUserRepository(engine,
				connectionString);
		}

		public UserUpdater(DatabaseEngineConstant engine,
			string connectionString) :
			base(engine, connectionString)
		{
			userRepository = UserRepoFactory.GetUserRepository(engine,
				connectionString);
		}

		public UserUpdater(IUserRepository userRepo)
		{
			this.userRepository = userRepo;
		}

		public void UpdateUser(INewUserData userContract)
		{
			if (string.IsNullOrEmpty(userContract.UserName))
			{
				string message = userContract == null ?
					"The user contract, recieved is null." : "Username is empty";
				new ApplicationException(message);
			}
			DbUser user = GetUser(userContract.UserName) ??
				throw new ApplicationException($"Could not find user with" +
					$" username: {userContract.UserName}");
			UpdateUserData(user, userContract);
			userRepository.UpdateUser(user);
		}

		private DbUser GetUser(string userName)
		{
			return userRepository.GetUserMatchingUsername(userName);
		}

		private void UpdateUserData(DbUser user, INewUserData userContract)
		{
			if (stringCheck.IsNotNullEmpty(userContract.FirstName))
			{
				user.FirstName = userContract.FirstName;
			}
			if (stringCheck.IsNotNullEmpty(userContract.Surname))
			{
				user.Surname = userContract.Surname;
			}
			if (stringCheck.IsNotNullEmpty(userContract.EmailAddress))
			{
				user.EmailAddress = userContract.EmailAddress;
			}
		}
	}
}