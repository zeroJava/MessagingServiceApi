using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.User;
using System;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class UpdateUserDataServiceLogic : LogicBase
	{
		public void UpdateUser(string token, NewUserData user)
		{
			try
			{
				ValidateToken(token);
				LogMethodICalled(nameof(UpdateUser));
				UserUpdater updateUserDataBL = new UserUpdater();
				updateUserDataBL.UpdateUser(user);
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