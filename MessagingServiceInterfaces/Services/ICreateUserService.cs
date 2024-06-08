using MessagingServiceInterfaces.Contracts.User;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICreateUserService" in both code and config file together.
	[ServiceContract]
	public interface ICreateUserService
	{
		[OperationContract]
		void CreateNewUser(NewUserData user);

		[OperationContract]
		void CreateNewAdvancedUser(NewAdvancedUserData user);
	}
}