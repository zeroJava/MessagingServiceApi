using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityClasses;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System;
using System.ServiceModel;
using WMessageServiceApi.Exceptions.Datacontacts;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
    public class UpdateUserDataServiceBl
    {
        public void UpdateUser(INewUserDataContract userContract)
        {
            if (userContract == null ||
                userContract.UserName == null ||
                userContract.UserName == string.Empty)
            {
                string message = userContract == null ? "The user contract, recieved is null." : "Username is empty";
                ThrowEntityErrorMessage(message, StatusList.PROCESS_ERROR);
            }
            User user = GetUserEntityObject(userContract.UserName);
            if (user == null)
            {
                throw new InvalidOperationException(string.Format("Could not find user with username: {0}", userContract.UserName));
            }
            UpdateUserEntityObject(user, userContract);
            IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
            userRepo.UpdateUser(user);
        }

        private User GetUserEntityObject(string userName)
        {
            IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString); ;
            return userRepo.GetUserMatchingUsername(userName);
        }

        private void UpdateUserEntityObject(User userEntityObject, INewUserDataContract userContract)
        {
            if (userContract.FirstName != null && userContract.FirstName != string.Empty)
            {
                userEntityObject.FirstName = userContract.FirstName;
            }
            if (userContract.Surname != null && userContract.Surname != string.Empty)
            {
                userEntityObject.Surname = userContract.Surname;
            }
            if (userContract.EmailAddress != null && userContract.EmailAddress != string.Empty)
            {
                userEntityObject.EmailAddress = userContract.EmailAddress;
            }
        }

        private void ThrowEntityErrorMessage(string message, int status)
        {
            ErrorContract error = new ErrorContract(message, status);
            throw new FaultException<ErrorContract>(error);
        }
    }
}