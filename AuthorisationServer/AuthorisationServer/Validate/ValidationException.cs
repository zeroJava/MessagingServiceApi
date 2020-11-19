using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorisationServer.Validate
{
	public class ValidationException : Exception
	{
		public int Status { get; set; }

		public ValidationException(string message, int status) : base(message)
		{
			Status = status;
		}
	}
}