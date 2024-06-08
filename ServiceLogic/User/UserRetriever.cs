using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Contracts.User;
using System.Collections.Generic;

using DbUser = MessageDbCore.RepoEntity.User;

namespace MessagingServiceFunctions.User
{
	public class UserRetriever : FunctionBase
	{
		private readonly IUserRepository userRepository;

		public UserRetriever()
		{
			userRepository = UserRepoFactory.GetUserRepository(engine,
				connectionString);
		}

		public List<UserInfo> GetAllUsers()
		{
			List<DbUser> users = userRepository.GetAllUsers();
			return CreateUserList(users);
		}

		private List<UserInfo> CreateUserList(List<DbUser> users)
		{
			if (users == null)
				return null;
			List<UserInfo> userList = new List<UserInfo>();
			foreach (DbUser user in users)
				userList.Add(ConvertToUserInfo(user));
			return userList;
		}

		private UserInfo ConvertToUserInfo(DbUser user)
		{
			if (user is AdvancedUser advancedUser)
				return new AdvancedUserInfo
				{
					UserName = advancedUser.UserName,
					EmailAddress = advancedUser.EmailAddress,
					FirstName = advancedUser.FirstName,
					Surname = advancedUser.Surname,
					Dob = advancedUser.DOB,
					Gender = advancedUser.Gender,
					AdvanceStartDatetime = advancedUser.AdvanceStartDatetime,
					AdvanceEndDatetime = advancedUser.AdvanceEndDatetime
				};
			return new UserInfo
			{
				UserName = user.UserName,
				EmailAddress = user.EmailAddress,
				FirstName = user.FirstName,
				Surname = user.Surname,
				Dob = user.DOB,
				Gender = user.Gender,
			};
		}

		public UserInfo GetUserMatchingUsername(string username)
		{
			DbUser user = userRepository
				.GetUserMatchingUsername(username);
			return ConvertToUserInfo(user);
		}
	}
}