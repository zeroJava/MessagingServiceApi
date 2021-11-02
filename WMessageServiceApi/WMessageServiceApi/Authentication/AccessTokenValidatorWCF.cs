using System;
using WMessageServiceApi.ValidationService;

namespace WMessageServiceApi.Authentication
{
    public class AccessTokenValidatorWcf : IAccessTokenValidator
    {
        public TokenValidationResult IsTokenValid(string encryptedToken)
        {
            TokenValidationResult tokenResult = new TokenValidationResult();

            ValidationResponse result = Validate(encryptedToken);
            tokenResult.IsValidationSuccess = result.ValidationIsSuccess;
            tokenResult.Message = result.Message;
            tokenResult.Status = result.Status;

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

            ValidationResponse result = ValidateUserCredential(encryptedUserCred);
            tokenResult.IsValidationSuccess = result.ValidationIsSuccess;
            tokenResult.Message = result.Message;
            tokenResult.Status = result.Status;

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