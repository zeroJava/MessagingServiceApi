using Cryptography;
using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.DbRepositoryFactories;
using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.IContracts.User;
using System;

using DbUser = MessageDbCore.RepoEntity.User;

namespace MessagingServiceFunctions.User
{
	public class UserCreator : FunctionBase
	{
		public void CreateNewAdvancedUser(NewAdvancedUserData advanceUser)
		{
			UsernameChecker.Check(advanceUser.UserName);
			DbUser user = NewUser(advanceUser);
			SaveUser(user);
			//PersistUserToMongoDbService(user);
		}

		public void CreateNewUser(INewUserData userContract)
		{
			UsernameChecker.Check(userContract.UserName);
			DbUser user = NewUser(userContract);
			SaveUser(user);
			//PersistUserToMongoDbService(user);
		}

		private DbUser NewUser(INewUserData userContract)
		{
			if (userContract is NewAdvancedUserData)
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
				DbUser user = new DbUser();
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

		private void SaveUser(DbUser user)
		{
			IUserRepository userRepo = UserRepoFactory.GetUserRepository(engine,
				connectionString);
			userRepo.InsertUser(user);
		}

		/*private void PersistUserToMongoDbService(User user)
		 {
			 RabbitMqProducerClass rabbitMqProducer = new
				RabbitMqProducerClass(QueueTypeConstant.MongoDbPersistentUserService,
					QueueTypeConstant.MongoDbPersistentUserService);
			 rabbitMqProducer.ExecuteMessageQueueing(user);
		 }*/
	}
}