using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class CreateUserService : ServiceBase, ICreateUserService
	{
		public void CreateNewAdvancedUser(NewAdvancedUserData user)
		{
			var createUserService = new ServiceLogics.CreateUserService();
			createUserService.CreateNewAdvancedUser(GetToken(), user);
		}

		public void CreateNewUser(NewUserData user)
		{
			var createUserService = new ServiceLogics.CreateUserService();
			createUserService.CreateNewUser(GetToken(), user);
		}
	}
}