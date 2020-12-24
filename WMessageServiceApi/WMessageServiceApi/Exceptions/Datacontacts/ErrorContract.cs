using System.Runtime.Serialization;

namespace WMessageServiceApi.Exceptions.Datacontacts
{
    [DataContract(Name = "ErrorContract")]
    public class ErrorContract : IErrorsContract
    {
        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        public ErrorContract(string message)
        {
            Message = message;
        }

        public ErrorContract(string message, int status)
        {
            Message = message;
            Status = status;
        }
    }
}