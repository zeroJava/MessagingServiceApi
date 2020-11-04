using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public class TokenValidationException : Exception
	{
		public TokenFailReason? Reason { get; set; }

		public TokenValidationException(string message, TokenFailReason? reason) : base(message)
		{
			this.Reason = reason;
		}
	}
}