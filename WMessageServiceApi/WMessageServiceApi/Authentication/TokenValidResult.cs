using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public struct TokenValidResult
	{
		public bool IsValidationSuccess { get; set; }
		public TokenFailReason? Reason { get; set; }
		public string Message { get; set; }
	}
}