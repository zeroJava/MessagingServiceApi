using System.Runtime.Serialization;

namespace AuthorisationServer.Validation
{
   [DataContract(Name = "ValidationResponse")]
   public class ValidationResponse
   {
      [DataMember(Name = "ValidationIsSuccess")]
      public bool ValidationIsSuccess { get; set; }

      [DataMember(Name = "Message")]
      public string Message { get; set; }

      [DataMember(Name = "Status")]
      public int Status { get; set; }
   }
}