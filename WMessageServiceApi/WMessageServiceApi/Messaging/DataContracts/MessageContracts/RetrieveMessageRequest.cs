using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
	[DataContract]
	public class RetrieveMessageRequest : IRetrieveMessageRequest
	{
		[DataMember]
		public string UserCredentials { get; set; }

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