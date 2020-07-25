using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMessageServiceApi.Messaging.DataContracts.LoginContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoginService" in both code and config file together.
	[ServiceContract]
	public interface ILoginService
	{
		[OperationContract]
		LoginTokenContract ExecuteEncryptedLoginIn(string encryptedUser, string encryptedPassword);
	}
}
