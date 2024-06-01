namespace WMessageServiceApi.Messaging.Facades
{
	public abstract class BaseFacade
	{
		/*protected virtual void ValidToken(string encryptedToken)
		{
			string option = AccessTokenValidatorFactory.ACCESS_TOKEN_WCF;
			IAccessTokenValidator tokenValidator = AccessTokenValidatorFactory
				.GetAccessTokenValidator(option);
			TokenValidationResult result = tokenValidator.IsTokenValid(encryptedToken);
			if (result.IsValidationSuccess)
			{
				return;
			}
			throw new TokenValidationException(result.Message, result.Status);
		}*/
	}
}