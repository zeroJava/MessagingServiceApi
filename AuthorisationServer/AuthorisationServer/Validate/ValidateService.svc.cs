using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AuthorisationServer.Validate
{
	public class ValidateService : IValidateService
	{
		public ValidationResult AccessTokenValidation(string encryptedToken)
		{
			try
			{
				ValidateServiceBL validateService = new ValidateServiceBL();
				ValidationResult result = validateService.AccessTokenValidation(encryptedToken);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}

		public ValidationResult UserCredentialValidation(string encryptedCredential)
		{
			try
			{
				ValidateServiceBL validateService = new ValidateServiceBL();
				ValidationResult result = validateService.UserCredentialValidation(encryptedCredential);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}
	}
}
