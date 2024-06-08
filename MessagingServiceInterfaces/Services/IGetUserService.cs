using MessagingServiceInterfaces.Contracts.User;
using System.Collections.Generic;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGetUserService" in both code and config file together.
	[ServiceContract]
	public interface IGetUserService
	{
		[OperationContract]
		List<UserInfo> GetAllUsers();

		//[OperationContract]
		//UserInfo GetUserMatchingUsernamePassword(string username, string password);
	}
}