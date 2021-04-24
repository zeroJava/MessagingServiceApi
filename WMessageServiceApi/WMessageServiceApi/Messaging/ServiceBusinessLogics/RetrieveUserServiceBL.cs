using MessageDbCore.DbRepositoryInterfaces;
using MessageDbCore.RepoEntity;
using MessageDbLib.Configurations;
using MessageDbLib.DbRepositoryFactories;
using System.Collections.Generic;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
    public class RetrieveUserServiceBl
    {
        public List<UserInfoContract> GetAllUsers()
        {
            IUserRepository userRepo = GetUserRepository();
            List<User> users = userRepo.GetAllUsers();
            return CreateUserContractList(users);
        }

        private IUserRepository GetUserRepository()
        {
            return UserRepoFactory.GetUserRepository(DatabaseOption.DatabaseEngine, DatabaseOption.DbConnectionString);
        }

        private List<UserInfoContract> CreateUserContractList(List<User> users)
        {
            if (users == null)
            {
                return null;
            }

            List<UserInfoContract> userList = new List<UserInfoContract>();
            foreach (User user in users)
            {
                if (user is AdvancedUser advancedUser)
                {
                    userList.Add(ConvertAdvancedUserToContract(advancedUser));
                    continue;
                }
                userList.Add(ConvertUserToContract(user));
            }
            return userList;
        }

        private UserInfoContract ConvertUserToContract(User user)
        {
            return new UserInfoContract
            {
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                Surname = user.Surname,
                Dob = user.DOB,
                Gender = user.Gender,
            };
        }

        private AdvancedUserInfoContract ConvertAdvancedUserToContract(AdvancedUser advancedUser)
        {
            return new AdvancedUserInfoContract
            {
                UserName = advancedUser.UserName,
                EmailAddress = advancedUser.EmailAddress,
                FirstName = advancedUser.FirstName,
                Surname = advancedUser.Surname,
                Dob = advancedUser.DOB,
                Gender = advancedUser.Gender,
                AdvanceStartDatetime = advancedUser.AdvanceStartDatetime,
                AdvanceEndDatetime = advancedUser.AdvanceEndDatetime
            };
        }

        public UserInfoContract GetUserMatchingUsernamePassword(string username, string password)
        {
            IUserRepository userRepo = GetUserRepository();
            User user = userRepo.GetUserMatchingUsernameAndPassword(username, password);
            if (user is AdvancedUser advancedUser)
            {
                return ConvertAdvancedUserToContract(advancedUser);
            }
            return ConvertUserToContract(user);
        }
    }
}