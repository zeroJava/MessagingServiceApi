using System.Runtime.Serialization;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	[DataContract]
	public class RetrieveMessageRequest : IRetrieveMessageRequest
	{
		[DataMember]
		public string UserAccessToken { get; set; }

		[DataMember]
		public string Username { get; set; }

		[DataMember]
		public string SenderEmailAddress { get; set; }

		[DataMember]
		public string ReceiverEmailAddress { get; set; }

		[DataMember]
		public long MessageIdThreshold { get; set; }

		[DataMember]
		public int NumberOfMessages { get; set; }
	}
}