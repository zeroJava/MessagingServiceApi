using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;
using WMessageServiceApi.Messaging.Facades;
using WMessageServiceApi.Messaging.ServiceInterfaces;

namespace WMessageServiceApi.Messaging.Services
{
	public class RetrieveUserService : IRetrieveUserService
	{
		public List<UserInfoContract> GetAllUsers()
		{
			try
			{
				RetrieveUserServiceFacade retrieveUserBL = new RetrieveUserServiceFacade();
				return retrieveUserBL.GetAllUsers();
			}
			catch (Exception exception)
			{
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ProcessError);
				throw new FaultException<ErrorContract>(error);
			}
		}

		public UserInfoContract GetUserMatchingUsernamePassword(string username, string password)
		{
			try
			{
				RetrieveUserServiceFacade retrieveUserBL = new RetrieveUserServiceFacade();
				return retrieveUserBL.GetUserMatchingUsernamePassword(username, password);
			}
			catch (Exception exception)
			{
				ErrorContract error = new ErrorContract(exception.Message, StatusList.ProcessError);
				throw new FaultException<ErrorContract>(error);
			}
		}
	}
}
