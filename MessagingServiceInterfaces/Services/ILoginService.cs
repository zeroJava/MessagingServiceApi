using MessagingServiceInterfaces.Contracts.Login;
using System.ServiceModel;

namespace MessagingServiceInterfaces.Services
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoginService" in both code and config file together.
	[ServiceContract]
	public interface ILoginService
	{
		[OperationContract]
		LoginToken ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword);
	}
}