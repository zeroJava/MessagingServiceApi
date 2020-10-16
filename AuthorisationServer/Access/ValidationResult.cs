using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Access
{
	[DataContract(Name = "ValidationResult")]
	public class ValidationResult
	{
		[DataMember(Name = "IsTokenValid")]
		public bool IsTokenValid { get; set; }

		[DataMember(Name = "Reason")]
		public string Reason { get; set; }
	}
}