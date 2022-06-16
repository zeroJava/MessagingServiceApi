using System.Collections.Generic;
using System.ServiceModel;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRetrieveUserService" in both code and config file together.
	[ServiceContract]
	public interface IRetrieveUserService
	{
		[OperationContract]
		List<UserInfoContract> GetAllUsers();

		[OperationContract]
		UserInfoContract GetUserMatchingUsernamePassword(string username, string password);
	}
}
