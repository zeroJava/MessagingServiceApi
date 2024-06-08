using System;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.User
{
	[DataContract(Name = "AdvancedUserInfo")]
	public class AdvancedUserInfo : UserInfo
	{
		[DataMember(Name = "AdvanceStartDatetime")]
		public DateTime? AdvanceStartDatetime { get; set; }

		[DataMember(Name = "AdvanceEndDatetime")]
		public DateTime? AdvanceEndDatetime { get; set; }
	}
}