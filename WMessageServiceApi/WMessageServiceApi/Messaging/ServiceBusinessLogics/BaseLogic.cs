using System;
using WMessageServiceApi.Authentication;
using WMessageServiceApi.Logging;

namespace WMessageServiceApi.Messaging.ServiceBusinessLogics
{
	public abstract class BaseLogic
	{
		protected virtual void ValidateAccessToken(string encryptedToken)
		{
			string option = AccessTokenValidatorFactory.ACCESS_TOKEN_WCF;
			/*if (string.IsNullOrEmpty(userAccessToken))
			{
				Console.WriteLine("This is a debug bypass, will be removed later.");
				return;
			}*/
			IAccessTokenValidator tokenValidator = AccessTokenValidatorFactory.GetAccessTokenValidator(option);
			TokenValidationResult result = tokenValidator.IsTokenValid(encryptedToken);
			if (!result.IsValidationSuccess)
			{
				throw new TokenValidationException(result.Message, result.Status);
			}
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