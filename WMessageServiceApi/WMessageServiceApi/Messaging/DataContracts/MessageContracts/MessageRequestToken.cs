using System.Runtime.Serialization;
using WMessageServiceApi.Messaging.DataEnumerations;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
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