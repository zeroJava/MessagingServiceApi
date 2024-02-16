using System;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	public interface IMessageDispatchInfo
	{
		string SenderName { get; set; }
		string ReceiverName { get; set; }
		string MessageContent { get; set; }
		DateTime? MessageSentDate { get; set; }
		DateTime? MessageReceivedDate { get; set; }
		bool? MessageReceived { get; set; }
		bool SenderCurrentUser { get; set; }
		long DispatchId { get; set; }
		long MessageId { get; set; }
	}
}
