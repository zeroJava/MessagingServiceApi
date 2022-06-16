using MessageDbCore.RepoEntity;
using System;
using System.Collections.Generic;

namespace MessageDbCore.DbRepositoryInterfaces
{
	public interface IUserRepository : IDisposable
	{
		void InsertUser(User user);
		void UpdateUser(User user);
		void DeleteUser(User user);

		User GetUserMatchingId(long userId);
		User GetUserMatchingUsername(string username);
		User GetUserMatchingUsernameAndPassword(string username, string password);
		List<User> GetAllUsers();
	}
}