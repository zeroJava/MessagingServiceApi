using MessagingServiceApi.ValidationService;

namespace MessagingServiceApi.Authentication
{
	public class AccessTokenValidatorWcf : IAccessTokenValidator
	{
		public TokenValidationResult Validate(string encryptedToken)
		{
			ValidationResponse result = ValidateToken(encryptedToken);
			return new TokenValidationResult
			{
				IsValidationSuccess = result.ValidationIsSuccess,
				Message = result.Message,
				Status = result.Status,
			};
		}

		private ValidationResponse ValidateToken(string encrptedToken)
		{
			ValidationServiceClient serviceClient =
				new ValidationServiceClient();
			ValidationResponse response =
				serviceClient.AccessTokenValidation(encrptedToken);
			return response;
		}

		public TokenValidationResult IsUserValid(string encryptedUserCred)
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