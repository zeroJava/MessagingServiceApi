using MessagingServiceInterfaces.Contracts.User;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUpdateUserService" in both code and config file together.
	[ServiceContract]
	public interface IUpdateUserService
	{
		[OperationContract]
		void UpdateUser(NewUserData user);

		//[OperationContract]
		//void UpdateUserEntityFramework(NewUserData userContract);
	}
}
