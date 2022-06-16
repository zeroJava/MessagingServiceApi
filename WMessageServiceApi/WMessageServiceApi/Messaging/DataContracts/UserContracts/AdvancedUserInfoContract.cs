using System;
using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.UserContracts
{
	[DataContract(Name = "AdvancedUserInfoContract")]
	public class AdvancedUserInfoContract : UserInfoContract
	{
		[DataMember(Name = "AdvanceStartDatetime")]
		public DateTime? AdvanceStartDatetime { get; set; }

		[DataMember(Name = "AdvanceEndDatetime")]
		public DateTime? AdvanceEndDatetime { get; set; }
	}
}