using System;
using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	[DataContract]
	public class MessageDispatchInfo : IMessageDispatchInfo
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