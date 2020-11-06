using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WMessageServiceApi.Messaging.DataContracts.UserContracts;

namespace WMessageServiceApi.Messaging.ServiceInterfaces
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUpdateUserService" in both code and config file together.
	[ServiceContract]
	public interface IUpdateUserService
	{
		[OperationContract]
		void UpdateUser(NewUserDataContract userContract);

		//[OperationContract]
		//void UpdateUserEntityFramework(NewUserDataContract userContract);
	}
}
