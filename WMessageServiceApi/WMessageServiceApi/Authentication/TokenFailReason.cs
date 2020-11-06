using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Authentication
{
	[DataContract]
	public enum TokenFailReason
	{
		[EnumMember]
		OrganisationNameEmpty = 1,

		[EnumMember]
		OrganisationNonFound = 2,

		[EnumMember]
		AccessTokenEmpty = 3,

		[EnumMember]
		AccessTokenExpired = 4,

		[EnumMember]
		TokenExtractError = 5,

		[EnumMember]
		TokenValuesDontMatch = 6,

		[EnumMember]
		TokenRowNotFound = 7,
	}
}