using System;

namespace MessagingServiceInterfaces.IContracts.Message
{
	public interface IPostedMessageInfo
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
