using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Access
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccessTokenService" in both code and config file together.
	[ServiceContract]
	public interface IAccessService
	{
		[OperationContract]
		AccessToken GetAccessToken(AccessRequest accessRequest);

		[OperationContract]
		AccessToken GetAccessTokenImplicit(string encryptedUsername, string encryptedPassword);
	}
}