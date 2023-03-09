using WMessageServiceApi.ValidationService;

namespace WMessageServiceApi.Authentication
{
	public class AccessTokenValidatorWcf : IAccessTokenValidator
	{
		public TokenValidationResult IsTokenValid(string encryptedToken)
		{
			ValidationResponse result = Validate(encryptedToken);
			return new TokenValidationResult
			{
				IsValidationSuccess = result.ValidationIsSuccess,
				Message = result.Message,
				Status = result.Status,
			};
		}

		private ValidationResponse Validate(string encrptedToken)
		{
			ValidationServiceClient serviceClient = new ValidationServiceClient();
			ValidationResponse response = serviceClient.AccessTokenValidation(encrptedToken);
			return response;
		}

		public TokenValidationResult IsUserCredentialValid(string encryptedUserCred)
		{
			ValidationResponse result = ValidateUserCredential(encryptedUserCred);
			return new TokenValidationResult
			{
				IsValidationSuccess = result.ValidationIsSuccess,
				Message = result.Message,
				Status = result.Status
			};
		}

		private ValidationResponse ValidateUserCredential(string encrptedToken)
		{
			ValidationServiceClient serviceClient = new ValidationServiceClient();
			ValidationResponse response = serviceClient.UserCredentialValidation(encrptedToken);
			return response;
		}
	}
}