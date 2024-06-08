using MessagingServiceInterfaces.Constants;

namespace MessagingServiceInterfaces.IContracts.Message
{
	public interface IMessageRequestToken
	{
		long MessageId { get; set; }
		string Message { get; set; }
		MessageReceivedState MessageRecievedState { get; set; }
	}
}