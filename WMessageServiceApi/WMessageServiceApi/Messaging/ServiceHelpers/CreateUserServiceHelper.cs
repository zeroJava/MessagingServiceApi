using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using WMessageServiceApi.Messaging.Facades;

namespace WMessageServiceApi.Messaging.ServiceHelpers
{
	public class CreateUserServiceHelper : BaseHelper
	{
		public void CreateNewAdvancedUser(string token,
			NewAdvancedUserDataContract advanceUserContract)
		{
			try
			{
				ValidToken(token);
				CreateUserFacade.CreateNewAdvancedUser(advanceUserContract);
			}
			catch (InvalidOperationException exception)
			{
				ThrowUserExistErrorMessage(exception.Message);
			}
			catch (Exception exception)
			{
				ThrowErrorMessage(exception.Message, StatusList.ProcessError);
			}
		}

		public void CreateNewUser(string token, NewUserDataContract userContract)
		{
			try
			{
				ValidToken(token);
				CreateUserFacade.CreateNewUser(userContract);
			}
			catch (InvalidOperationException exception)
			{
				ThrowUserExistErrorMessage(exception.Message);
			}
			catch (Exception exception)
			{
				ThrowErrorMessage(exception.Message, StatusList.ProcessError);
			}
		}

		private void ThrowErrorMessage(string message, int status)
		{
			ErrorContract error = new ErrorContract(message, status);
			throw new FaultException<ErrorContract>(error);
		}

		private void ThrowUserExistErrorMessage(string message)
		{
			UserExistErrorContract error = new UserExistErrorContract
			{
				Message = message
			};
			throw new FaultException<UserExistErrorContract>(error);
		}
	}
}