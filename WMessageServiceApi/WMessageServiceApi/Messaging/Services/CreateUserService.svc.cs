using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class CreateUserService : ServiceBase, ICreateUserService
	{
		public void CreateNewAdvancedUser(NewAdvancedUserData user)
		{
			CreateUserServiceLogic serviceHelper = new CreateUserServiceLogic();
			serviceHelper.CreateNewAdvancedUser(GetToken(), user);
		}

		public void CreateNewUser(NewUserData user)
		{
			CreateUserServiceLogic serviceHelper = new CreateUserServiceLogic();
			serviceHelper.CreateNewUser(GetToken(), user);
		}
	}
}
