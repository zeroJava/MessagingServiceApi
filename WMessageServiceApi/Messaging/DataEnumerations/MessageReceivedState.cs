using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Messaging.DataEnumerations
{
	[DataContract]
	public enum MessageReceivedState
	{
		[EnumMember]
		AcknowledgedRequest,

		[EnumMember]
		FailedToProcessRequest,
	}
}