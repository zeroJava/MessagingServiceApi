using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.EntityClasses;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using MessageDbLib.SecurityOperations;
using System;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
    public class CreateUserServiceBL
    {
        public void CreateNewAdvancedUser(NewAdvancedUserDataContract advanceUserContract)
        {
            if (advanceUserContract.UserName != null &&
                advanceUserContract.UserName != "" &&
                UsernameAlreadyExist(advanceUserContract.UserName))
            {
                throw new InvalidOperationException("This username has already been taken.");
            }
            User user = ConstructUser(advanceUserContract);
            PersistNewUser(user);
            //PersistUserToMongoDbService(user);
        }

        public void CreateNewUser(INewUserDataContract userContract)
        {
            if (userContract.UserName != null && userContract.UserName != "" && UsernameAlreadyExist(userContract.UserName))
            {
                throw new InvalidOperationException("This username has already been taken.");
            }
            User user = ConstructUser(userContract);
            PersistNewUser(user);
            //PersistUserToMongoDbService(user);
        }

        private User ConstructUser(INewUserDataContract userContract)
        {
            if (userContract is NewAdvancedUserDataContract)
            {
                AdvancedUser advancedUser = new AdvancedUser();
                advancedUser.UserName = userContract.UserName;
                advancedUser.Password = SecurityHashWrapper.Get512HashString(userContract.Password);
                advancedUser.EmailAddress = userContract.Password;
                advancedUser.FirstName = userContract.FirstName;
                advancedUser.Surname = userContract.Surname;
                advancedUser.DOB = userContract.Dob;
                advancedUser.Gender = userContract.Gender;
                advancedUser.AdvanceStartDatetime = DateTime.Now;
                advancedUser.AdvanceEndDatetime = DateTime.Now.AddYears(1);
                return advancedUser;
            }
            else
            {
                User user = new User();
                user.UserName = userContract.UserName;
                user.Password = SecurityHashWrapper.Get512HashString(userContract.Password);
                user.EmailAddress = userContract.Password;
                user.FirstName = userContract.FirstName;
                user.Surname = userContract.Surname;
                user.DOB = userContract.Dob;
                user.Gender = userContract.Gender;
                return user;
            }
        }

        private void PersistNewUser(User user)
        {
            IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
            userRepo.InsertUser(user);
        }

        /*private void PersistUserToMongoDbService(User user)
		{
			RabbitMqProducerClass rabbitMqProducer = new RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
				QueueTypeConstant.MongoDbPersistentUserService);
			rabbitMqProducer.ExecuteMessageQueueing(user);
		}*/

        private bool UsernameAlreadyExist(string userName)
        {
            IUserRepository userRepo = UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
            //Func<User, bool> funcQuery = u => string.Equals(u.USERNAME, userName, StringComparison.InvariantCultureIgnoreCase);
            User user = userRepo.GetUserMatchingUsername(userName);
            return user != null;
        }
    }
}