using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.User;
using System;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class UpdateUserDataService : LogicBase
	{
		public void UpdateUser(string token, NewUserData user)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(UpdateUser));
				UserUpdater userUpdater = new UserUpdater();
				userUpdater.UpdateUser(user);
			}
			catch (TokenValidationException) { throw; }
			catch (Exception exception)
			{
				LogError(exception, nameof(UpdateUser));
				throw processError.Invoke(exception.Message);
			}
		}
	}
}