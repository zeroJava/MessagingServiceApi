using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMessageServiceApi.ValidationService;

namespace WMessageServiceApi.Authentication
{
	public class AccessTokenValidatorWCF : IAccessTokenValidator
	{
		public TokenValidationResult IsTokenValid(string encryptedToken)
		{
			TokenValidationResult tokenResult = new TokenValidationResult();
			try
			{
				ValidationResponse result = Validate(encryptedToken);
				tokenResult.IsValidationSuccess = result.ValidationIsSuccess;
				tokenResult.Message = result.Message;
				tokenResult.Status = result.Status;
			}
			catch (Exception exception)
			{
				throw;
			}
			return tokenResult;
		}

		private ValidationResponse Validate(string encrptedToken)
		{
			ValidationServiceClient serviceClient = new ValidationServiceClient();
			ValidationResponse response = serviceClient.AccessTokenValidation(encrptedToken);
			return response;
		}

		public TokenValidationResult IsUserCredentialValid(string encryptedUserCred)
		{
			TokenValidationResult tokenResult = new TokenValidationResult();
			try
			{
				ValidationResponse result = ValidateUserCredential(encryptedUserCred);
				tokenResult.IsValidationSuccess = result.ValidationIsSuccess;
				tokenResult.Message = result.Message;
				tokenResult.Status = result.Status;
			}
			catch (Exception exception)
			{
				throw;
			}
			return tokenResult;
		}

		private ValidationResponse ValidateUserCredential(string encrptedToken)
		{
			ValidationServiceClient serviceClient = new ValidationServiceClient();
			ValidationResponse response = serviceClient.UserCredentialValidation(encrptedToken);
			return response;
		}
	}
}