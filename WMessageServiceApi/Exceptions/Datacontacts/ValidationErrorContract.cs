using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WMessageServiceApi.Authentication;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
	[DataContract(Name = "ValidationErrorContract")]
	public class ValidationErrorContract : IErrorsContract
	{
		[DataMember(Name = "Reason")]
		public TokenFailReason? Reason { get; set; }

		[DataMember(Name = "Message")]
		public string Message { get; set; }
	}
}