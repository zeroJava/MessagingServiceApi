using MessagingServiceApi.Messaging.ServiceLogics;
using MessagingServiceInterfaces.Contracts.User;
using MessagingServiceInterfaces.Services;
using System.Collections.Generic;

namespace MessagingServiceApi.Messaging.Services
{
	public class GetUserService : ServiceBase, IGetUserService
	{
		public List<UserInfo> GetAllUsers()
		{
			GetUserServiceLogic serviceHelper = new GetUserServiceLogic();
			return serviceHelper.GetAllUsers(GetToken());
		}
	}
}