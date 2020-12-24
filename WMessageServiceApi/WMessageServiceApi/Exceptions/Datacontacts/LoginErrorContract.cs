using System.Runtime.Serialization;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    [DataContract(Name = "LoginErrorContract")]
    public class LoginErrorContract : IErrorsContract
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }
    }
}