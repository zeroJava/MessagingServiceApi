using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Validation
{
	public sealed class StatusDictionary
	{
		// 100 - 199 means validation was a success.
		public const int SUCCESS = 100;

		// 200 - 499 means validation was unsuccessful.
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
	}
}