using MessageDbCore.DbRepositoryInterfaces;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;

using DbUser = MessageDbCore.RepoEntity.User;

namespace MessagingServiceFunctions.User
{
	public static class UsernameChecker
	{
		public static void Check(string username)
		{
			bool failed = false;
			string reason = string.Empty;
			try
			{
				if (string.IsNullOrEmpty(username))
				{
					reason = "Username empty";
					failed = true;
					return;
				}
				if (UsernameExist(username))
				{
					reason = "Username already taken";
					failed = true;
					return;
				}
			}
			catch
			{
				reason = "Error";
				failed = true;
				return;
			}
			finally
			{
				if (failed)
				{
					throw new ApplicationException(reason);
				}
			}
		}

		private static bool UsernameExist(string userName)
		{
			var engine = DatabaseOption.DatabaseEngine;
			var connectionstring = DatabaseOption.DbConnectionString;
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(engine,
				connectionstring);
			DbUser user = userRepo.GetUserMatchingUsername(userName);
			return user != null;
		}
	}
}