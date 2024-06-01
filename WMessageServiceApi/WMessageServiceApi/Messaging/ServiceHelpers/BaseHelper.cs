using System;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Logging;

namespace WMessageServiceApi.Messaging.ServiceHelpers
{
	public abstract class BaseHelper
	{
		protected virtual void ValidToken(string encryptedToken)
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
		}

		protected virtual void LogError(string message)
		{
			AppLog.LogError(message);
		}

		protected virtual void LogError(string message, Exception exception)
		{
			AppLog.LogError(message + "\n" + exception.ToString());
		}

		protected virtual void LogInfo(string message)
		{
			AppLog.LogInfo(message + ".");
		}
	}
}