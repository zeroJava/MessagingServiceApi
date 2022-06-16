using System.Runtime.Serialization;

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