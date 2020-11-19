using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public class AccessTokenValidatorWCF : IAccessTokenValidator
	{
		private Dictionary<ValidationFailReason, Func<TokenFailReason>> FailReasonDictionary =
			new Dictionary<ValidationFailReason, Func<TokenFailReason>>
		{
			{ ValidationFailReason.OrganisationNameEmpty, () => TokenFailReason.OrganisationNameEmpty },
			{ ValidationFailReason.OrganisationNonFound, () => TokenFailReason.OrganisationNonFound },
			{ ValidationFailReason.AccessTokenEmpty, () => TokenFailReason.AccessTokenEmpty },
			{ ValidationFailReason.AccessTokenExpired, () => TokenFailReason.AccessTokenExpired },
			{ ValidationFailReason.TokenExtractError, () => TokenFailReason.TokenExtractError },
			{ ValidationFailReason.TokenValuesDontMatch, () => TokenFailReason.TokenValuesDontMatch },
			{ ValidationFailReason.TokenRowNotFound, () => TokenFailReason.TokenRowNotFound },
		};

		public TokenValidResult IsTokenValid(string encryptedToken)
		{
			TokenValidResult tokenResult = new TokenValidResult();
			try
			{
				ValidationResult result = Validate(encryptedToken);
				tokenResult.IsValidationSuccess = result.IsTokenValid;
				tokenResult.Message = result.Message;
				tokenResult.Reason = GetTokenFailReason(result.FailReason);
			}
			catch (Exception exception)
			{
				throw;
			}
			return tokenResult;
		}

		private ValidationResult Validate(string encrptedToken)
		{
			AccessServiceClient serviceClient = new AccessServiceClient();
			ValidationResult result = serviceClient.CheckAccessTokenValid(encrptedToken);
			return result;
		}

		private TokenFailReason? GetTokenFailReason(ValidationFailReason? failReason)
		{
			if (failReason == null)
			{
				return null;
			}
			try
			{
				return FailReasonDictionary[failReason.Value].Invoke();
			}
			catch
			{
				return null;
			}
		}

		public TokenValidResult IsUserCredentialValid(string encryptedUserCred)
		{
			throw new NotImplementedException();
		}
	}
}