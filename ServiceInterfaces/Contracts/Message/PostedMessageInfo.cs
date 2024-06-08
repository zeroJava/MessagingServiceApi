using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Message
{
	[DataContract]
	public class PostedMessageInfo : IPostedMessageInfo
	{
		[DataMember]
		public string SenderName { get; set; }
		[DataMember]
		public string ReceiverName { get; set; }
		[DataMember]
		public DateTime? MessageSentDate { get; set; }
		[DataMember]
		public string MessageContent { get; set; }
		[DataMember]
		public bool? MessageReceived { get; set; }
		[DataMember]
		public DateTime? MessageReceivedDate { get; set; }
		[DataMember]
		public bool SenderCurrentUser { get; set; }
		[DataMember]
		public long DispatchId { get; set; }
		[DataMember]
		public long MessageId { get; set; }
	}
}