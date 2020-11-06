using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

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