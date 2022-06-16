using System.Runtime.Serialization;

namespace AuthorisationServer.Access
{
   [DataContract]
   public class OrganisationKeySerDes
   {
      [DataMember]
      public string Name { get; set; }

      [DataMember]
      public string OKey { get; set; }
   }
}