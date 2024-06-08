using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Constants
{
	[DataContract]
	public enum MessageReceivedState
	{
		[EnumMember]
		Successful,

		[EnumMember]
		FailedToProcess,
	}
}