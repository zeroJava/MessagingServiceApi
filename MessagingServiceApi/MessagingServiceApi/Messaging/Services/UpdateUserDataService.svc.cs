using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class UpdateUserDataService : ServiceBase, IUpdateUserService
	{
		public void UpdateUser(NewUserData user)
		{
			UpdateUserDataServiceLogic serviceHelper = new UpdateUserDataServiceLogic();
			serviceHelper.UpdateUser(GetToken(), user);
		}
	}
}