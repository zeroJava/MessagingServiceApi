using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;

namespace MessagingServiceApi.Messaging.Services
{
	public class UpdateUserDataService : ServiceBase, IUpdateUserService
	{
		public void UpdateUser(NewUserData user)
		{
			var serviceHelper = new ServiceLogics.UpdateUserDataService();
			serviceHelper.UpdateUser(GetToken(), user);
		}
	}
}