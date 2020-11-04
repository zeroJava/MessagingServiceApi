using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public static class AccessTokenValidatorFactory
	{
		public const string ACCESS_TOKEN_WCF = "WCF";
		public const string ACCESS_TOEKN_API = "API";

		public static IAccessTokenValidator GetAccessTokenValidator(string validationOption)
		{
			switch (validationOption)
			{
				case ACCESS_TOKEN_WCF:
					{
						AccessTokenValidatorWCF validatorWCF = new AccessTokenValidatorWCF();
						return validatorWCF;
					}
				default:
					{
						throw new ApplicationException("Access-Token-Validator option selected does not exist.");
					}
			}
		}
	}
}