using System;
using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.ServiceInterfaces;
using WMessageServiceApi.Messaging.ServiceBusinessLogics;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.Services
{
	public class RetrieveUserService : IRetrieveUserService
    {
        public List<UserInfoContract> GetAllUsers()
        {
            try
            {
				RetrieveUserServiceBL retrieveUserBL = new RetrieveUserServiceBL();
				return retrieveUserBL.GetAllUsers();
            }
            catch (Exception exception)
            {
                var error = new EntityErrorContract
                {
                    Message = exception.Message
                };
                throw new FaultException<EntityErrorContract>(error);
            }
        }

		public UserInfoContract GetUserMatchingUsernamePassword(string username, string password)
		{
			try
			{
				RetrieveUserServiceBL retrieveUserBL = new RetrieveUserServiceBL();
				return retrieveUserBL.GetUserMatchingUsernamePassword(username, password);
			}
			catch (Exception exception)
			{
				var error = new EntityErrorContract
				{
					Message = exception.Message
				};
				throw new FaultException<EntityErrorContract>(error);
			}
		}
    }
}
