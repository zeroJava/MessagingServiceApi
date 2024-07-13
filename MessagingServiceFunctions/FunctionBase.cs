using MessageDbLib.Configurations;
using MessageDbLib.Constants;

namespace MessagingServiceFunctions
{
	public abstract class FunctionBase
	{
		protected readonly DatabaseEngineConstant engine;
		protected readonly string connectionString;

		protected FunctionBase()
		{
			engine = DatabaseOption.DatabaseEngine;
			connectionString = DatabaseOption.DbConnectionString;
		}

		protected FunctionBase(DatabaseEngineConstant engine,
			string connectionString)
		{
			this.engine = engine;
			this.connectionString = connectionString;
		}

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