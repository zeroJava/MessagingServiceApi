using System;

namespace MessagingServiceApi.Authentication
{
	public class TokenValidationException : Exception
	{
		public const int PROPERTY_EMPTY = 451;
		public const int ACCESS_TOKEN_EMPTY = 454;
		public const int ACCESS_TOKEN_EXPIRED = 455;
		public const int EXTRACTION_ERROR = 456;
		public const int TOKEN_VALUE_DOES_NOT_MATCH = 457;

		public const int DECRYPTION_ERROR = 459;
		public const int STRING_VALUE_EMPTY = 460;

		public const int USER_NOT_FOUND = 471;
		public const int ORGANISATION_NOT_FOUND = 472;
		public const int TOKEN_NOT_FOUND = 473;

		public int Reason { get; set; }

		public TokenValidationException(string message, int reason) : base(message)
		{
			this.Reason = reason;
		}
	}
}