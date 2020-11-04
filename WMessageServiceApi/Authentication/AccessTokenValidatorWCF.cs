using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public class AccessTokenValidatorWCF : IAccessTokenValidator
	{
		public TokenValidResult IsTokenValid(string encryptedToken)
		{
			TokenValidResult tokenResult = new TokenValidResult();
			try
			{
				ValidationResult result = Validator(encryptedToken);
				tokenResult.IsValidationSuccess = result.IsTokenValid;
				//tokenResult.Message = result.mes
			}
			catch (Exception exception)
			{
				// need to update and bind
				throw;
			}
			return tokenResult;
		}

		private ValidationResult Validator(string encrptedToken)
		{
			AccessServiceClient serviceClient = new AccessServiceClient();
			ValidationResult result = serviceClient.CheckAccessTokenValid(encrptedToken);
			return result;
		}
	}
}