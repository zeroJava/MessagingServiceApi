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
				ValidationResponse result = new ValidationServiceBl().AccessTokenValidation(encryptedToken);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}

		public ValidationResponse UserCredentialValidation(string credential)
		{
			try
			{
				ValidationResponse result = new ValidationServiceBl().UserCredentialValidation(credential);
				return result;
			}
			catch (Exception exception)
			{
				throw new FaultException(exception.ToString());
			}
		}
	}
}
