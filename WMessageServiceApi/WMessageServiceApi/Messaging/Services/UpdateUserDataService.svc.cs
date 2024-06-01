using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using WMessageServiceApi.Messaging.Facades;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class UpdateUserDataService : IUpdateUserService
	{
		public void UpdateUser(NewUserDataContract userContract)
		{
			try
			{
				UpdateUserDataServiceFacade updateUserDataBL = new UpdateUserDataServiceFacade();
				updateUserDataBL.UpdateUser(userContract);
			}
			catch (Exception exception)
			{
				ThrowEntityErrorMessage(exception.Message, StatusList.ProcessError);
			}
		}

		private void ThrowEntityErrorMessage(string message, int status)
		{
			ErrorContract error = new ErrorContract(message, status);
			throw new FaultException<ErrorContract>(error);
		}
	}
}
