using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Access
{
	[DataContract(Name = "AccessToken")]
	public class AccessToken
	{
		[DataMember(Name = "UserAccount")]
		public string UserAccount { get; set; }

		[DataMember(Name = "Token")]
		public string Token { get; set; }
	}
}