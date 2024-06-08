using MessagingServiceApi.Authentication;
using MessagingServiceFunctions;
using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.Contracts.Errors;
using MessagingServiceInterfaces.Contracts.Message;
using System;
using System.ServiceModel;

namespace MessagingServiceApi.Messaging.ServiceLogics
{
	public abstract class LogicBase
	{
		protected readonly Func<string, FaultException<MessageRequestToken>> messageError = (string m) =>
		{
			var error = new MessageRequestToken
			{
				MessageRecievedState = MessageReceivedState.FailedToProcess,
				Message = m,
			};
			return new FaultException<MessageRequestToken>(error);
		};

		protected readonly Func<string, FaultException<Error>> processError = (string m) =>
		{
			Error error = new Error
			{
				Message = m,
				Status = StatusList.ProcessError,
			};
			return new FaultException<Error>(error);
		};

		protected readonly Error validationError = new Error
		{
			Message = "Error validating token",
			Status = StatusList.ValidationError,
		};

		protected virtual void ValidateToken(string encryptedToken)
		{
			string option = AccessTokenValidatorFactory.ACCESS_TOKEN_WCF;
			IAccessTokenValidator tokenValidator = AccessTokenValidatorFactory
				.GetAccessTokenValidator(option);
			TokenValidationResult result = tokenValidator.Validate(encryptedToken);
			if (result.IsValidationSuccess)
			{
				return;
			}
			throw new TokenValidationException(result.Message, result.Status);
		}

		protected virtual void LogError(string message)
		{
			Log.Error(message);
		}

		protected virtual void LogError(string message, Exception exception)
		{
			Log.Error(message + "\n" + exception.ToString());
		}

		protected virtual void LogError(Exception exception, string methodName = "Unknown")
		{
			Log.Error($"Error encountered in {methodName}.\n" + exception.ToString());
		}

		protected virtual void LogInfo(string message)
		{
			Log.Info(message + ".");
		}

		protected virtual void LogMethodICalled(string methodname)
		{
			Log.Info($"Method invoked: {methodname}");
		}
	}
}