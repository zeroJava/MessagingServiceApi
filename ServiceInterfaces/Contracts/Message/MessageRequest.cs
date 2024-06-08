using MessagingServiceInterfaces.IContracts.Message;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MessagingServiceInterfaces.Contracts.Message
{
	[DataContract]
	[KnownType(typeof(PictureMessageRequest))]
	[KnownType(typeof(VideoMessageRequest))]
	public class MessageRequest : IMessageRequest
	{
		[DataMember]
		public string AccessToken { get; set; }

		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public List<string> EmailAccounts { get; set; }

		[DataMember]
		public DateTime MessageCreated { get; set; }
	}
}