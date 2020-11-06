using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AuthorisationServer.Validate
{
	[DataContract]
	public enum FailReason
	{
		[EnumMember]
		ParameterEmtpy = 1,

		[EnumMember]
		OrganisationNameEmpty = 2,

		[EnumMember]
		OrganisationNonFound = 3,

		[EnumMember]
		AccessTokenEmpty = 4,

		[EnumMember]
		AccessTokenExpired = 5,

		[EnumMember]
		ExtractError = 6,

		[EnumMember]
		TokenValuesDontMatch = 7,

		[EnumMember]
		TokenRowNotFound = 8,

		[EnumMember]
		DecryptionError = 9,
	}
}