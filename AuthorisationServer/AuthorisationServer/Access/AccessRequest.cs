using System.Runtime.Serialization;

namespace AuthorisationServer.Access
{
   [DataContract(Name = "AccessRequest")]
   public class AccessRequest
   {
      [DataMember(Name = "Key")]
      public string Key { get; set; }

      [DataMember(Name = "AuthenticationCode")]
      public string AuthenticationCode { get; set; }

      [DataMember(Name = "Scope")]
      public string[] Scope { get; set; }
   }
}