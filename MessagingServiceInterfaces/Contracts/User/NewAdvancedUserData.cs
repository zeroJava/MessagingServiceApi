using System;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.User
{
	[DataContract(Name = "NewAdvancedUserData")]
	public class NewAdvancedUserData : NewUserData
	{
		[DataMember(Name = "AdvanceStartDatetime")]
		public DateTime? AdvanceStartDatetime { get; set; }

		[DataMember(Name = "AdvanceEndDatetime")]
		public DateTime? AdvanceEndDatetime { get; set; }
	}
}