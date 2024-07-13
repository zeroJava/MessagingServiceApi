using MessagingServiceApi.Authentication;
using MessagingServiceFunctions.User;
using MessagingServiceInterfaces.Contracts.Errors;
using MessagingServiceInterfaces.Contracts.User;
using System;
using System.ServiceModel;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public class CreateUserServiceLogic : LogicBase
	{
		protected readonly Func<string, FaultException<UserCheckError>> usercheckError = (string m) =>
		{
			var error = new UserCheckError
			{
				Message = m,
			};
			return new FaultException<UserCheckError>(error);
		};

		public void CreateNewAdvancedUser(string token, NewAdvancedUserData user)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(CreateNewAdvancedUser));
				UserCreator userCreator = new UserCreator();
				userCreator.CreateNewAdvancedUser(user);
			}
			catch (TokenValidationException) { throw; }
			catch (ApplicationException exception)
			{
				usercheckError.Invoke(exception.Message);
			}
			catch (Exception exception)
			{
				LogError(exception, nameof(CreateNewAdvancedUser));
				throw messageError.Invoke(exception.Message);
			}
		}

		public void CreateNewUser(string token, NewUserData user)
		{
			try
			{
				ValidateToken(token);
				LogMethodInvoked(nameof(CreateNewUser));
				UserCreator userCreator = new UserCreator();
				userCreator.CreateNewUser(user);
			}
			catch (TokenValidationException) { throw; }
			catch (ApplicationException exception)
			{
				usercheckError.Invoke(exception.Message);
			}
			catch (Exception exception)
			{
				LogError(exception, nameof(CreateNewUser));
				throw messageError.Invoke(exception.Message);
			}
		}
	}
}