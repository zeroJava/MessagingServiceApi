using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    [DataContract(Name = "UserExistErrorContract")]
    public class UserExistErrorContract : IErrorsContract
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}