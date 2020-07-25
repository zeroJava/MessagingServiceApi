using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    [DataContract(Name = "MessageQueueErrorContract")]
    public class MessageQueueErrorContract : IErrorsContract
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "ExceptionMessage")]
        public string ExceptionMessage { get; set; }
    }
}