using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Validation

{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IValidationService" in both code and config file together.
	[ServiceContract]
	public interface IValidationService
	{
		[OperationContract]
		ValidationResponse AccessTokenValidation(string encryptedToken);

		[OperationContract]
		ValidationResponse UserCredentialValidation(string credential);
	}
}
