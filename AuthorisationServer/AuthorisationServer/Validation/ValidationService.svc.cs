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
				LogError(exception.ToString());
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
				LogError(exception.ToString());
				throw new FaultException(exception.ToString());
			}
		}

		private static void LogError(string message)
		{
			Logging.AppLog.LogError(message);
		}
	}
}
