using MessageDbLib.Configurations;
using MessageDbLib.Constants;

namespace MessagingServiceFunctions
{
	public abstract class FunctionBase
	{
		protected readonly DatabaseEngineConstant engine = DatabaseOption.DatabaseEngine;
		protected readonly string connectionString = DatabaseOption.DbConnectionString;

		/*protected virtual void ValidateToken(string encryptedToken)
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