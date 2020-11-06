using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Access
{
	[DataContract]
	public class OrganisationKeySerDes
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string OKey { get; set; }
	}
}