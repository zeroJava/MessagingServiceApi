using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	public struct TokenValidationResult
	{
		public bool IsValidationSuccess { get; set; }
		public int Status { get; set; }
		public string Message { get; set; }
	}
}