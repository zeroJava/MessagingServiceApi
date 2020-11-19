using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Validate
{
	[DataContract(Name = "ValidationResult")]
	public class ValidationResult
	{
		[DataMember(Name = "ValidationIsSuccess")]
		public bool ValidationIsSuccess { get; set; }

		[DataMember(Name = "Message")]
		public string Message { get; set; }

		[DataMember(Name = "Status")]
		public int Status { get; set; }
	}
}