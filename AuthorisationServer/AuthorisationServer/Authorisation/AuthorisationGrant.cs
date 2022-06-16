using System;
using System.Runtime.Serialization;

namespace AuthorisationServer.Authorisation
{
   [DataContract(Name = "AuthorisationGrant")]
   public class AuthorisationGrant
   {
      [DataMember(Name = "AuthorisationCode")]
      public Guid AuthorisationCode { get; set; }

      [DataMember(Name = "Scope")]
      public string[] Scope { get; set; }

      [DataMember(Name = "Callback")]
      public string Callback { get; set; }
   }
}