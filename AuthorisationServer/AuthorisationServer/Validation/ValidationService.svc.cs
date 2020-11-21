using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Validation
{
	public class ValidationService : IValidationService
	{
		public ValidationResponse AccessTokenValidation(string encryptedToken)
		{
			try
			{
				ValidationServiceBL validateService = new ValidationServiceBL();
				ValidationResponse result = validateService.AccessTokenValidation(encryptedToken);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}

		public ValidationResponse UserCredentialValidation(string encryptedCredential)
		{
			try
			{
				ValidationServiceBL validateService = new ValidationServiceBL();
				ValidationResponse result = validateService.UserCredentialValidation(encryptedCredential);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}
	}
}
