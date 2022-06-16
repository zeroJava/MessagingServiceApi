using System.Runtime.Serialization;

namespace AuthorisationServer.Authorisation
{
   [DataContract]
   public class AuthorisationRequest
   {
      [DataMember]
      public string Username { get; set; }

      [DataMember]
      public string Password { get; set; }

      [DataMember]
      public string[] Scope { get; set; }

      [DataMember]
      public string Callback { get; set; }
   }
}