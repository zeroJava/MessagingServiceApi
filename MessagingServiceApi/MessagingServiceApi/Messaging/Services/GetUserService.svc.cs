using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;
using System.Collections.Generic;

namespace MessagingServiceApi.Messaging.Services
{
	public class GetUserService : ServiceBase, IGetUserService
	{
		public List<UserInfo> GetAllUsers()
		{
			var serviceHelper = new ServiceLogics.GetUserService();
			return serviceHelper.GetAllUsers(GetToken());
		}
	}
}