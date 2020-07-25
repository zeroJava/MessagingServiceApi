using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Messaging.DataContracts.MessageContracts
{
    [DataContract]
	[KnownType(typeof(PictureMessageContract))]
	[KnownType(typeof(VideoMessageContract))]
    public class MessageContract : IMessageContract
    {
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