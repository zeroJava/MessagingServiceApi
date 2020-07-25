using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    [DataContract(Name = "NonExistantUserErrorContract")]
    public class NonExistantUserErrorContract : IErrorsContract
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }
    }
}