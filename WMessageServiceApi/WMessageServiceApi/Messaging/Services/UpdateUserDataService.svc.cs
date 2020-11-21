using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;

namespace WMessageServiceApi.Messaging.Services
{
	public class UpdateUserDataService : IUpdateUserService
	{
		public void UpdateUser(NewUserDataContract userContract)
		{
			try
			{
				UpdateUserDataServiceBL updateUserDataBL = new UpdateUserDataServiceBL();
				updateUserDataBL.UpdateUser(userContract);
			}
			catch (Exception exception)
			{
				ThrowEntityErrorMessage(exception.Message, StatusList.PROCESS_ERROR);
			}
		}

		private void ThrowEntityErrorMessage(string message, int status)
		{
			ErrorContract error = new ErrorContract(message, status);
			throw new FaultException<ErrorContract>(error);
		}
	}
}
