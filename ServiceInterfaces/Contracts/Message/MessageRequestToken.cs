using MessagingServiceInterfaces.Constants;
using MessagingServiceInterfaces.IContracts.Message;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Message
{
	[DataContract]
	public class MessageRequestToken : IMessageRequestToken
	{
		[DataMember]
		public long MessageId { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public MessageReceivedState MessageRecievedState { get; set; }
	}
}