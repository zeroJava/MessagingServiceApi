using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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