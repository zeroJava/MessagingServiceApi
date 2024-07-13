using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.User;
using System;
using System.Collections.Generic;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class GetUserServiceLogic : LogicBase
	{
		public List<UserInfo> GetAllUsers(string token)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(GetAllUsers));
				UserRetriever userRetriever = new UserRetriever();
				return userRetriever.GetAllUsers();
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(GetAllUsers));
				throw processError.Invoke(exception.Message);
			}
		}
	}
}