using System;
using System.Runtime.Serialization;

namespace AuthorisationServer.Access
{
   [DataContract(Name = "AccessToken")]
   public class AccessToken
   {
      [DataMember(Name = "Organisation")]
      public string Organisation { get; set; }

      [DataMember(Name = "Token")]
      public string Token { get; set; }

      //[DataMember(Name = "UserId")]
      //public long UserId { get; set; }

      [DataMember(Name = "StartTime")]
      public DateTime StartTime { get; set; }

      [DataMember(Name = "EndTime")]
      public DateTime EndTime { get; set; }

      [DataMember(Name = "Scope")]
      public string[] Scope { get; set; }
   }
}